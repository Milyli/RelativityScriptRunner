namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class RDOMap
    {
        protected RDOMap(Type mappedType, RelativityDynamicObjectAttribute dynamicObjectAttribute)
        {
            this.MappedType = mappedType;
            this.TypeGuid = dynamicObjectAttribute.Guid;
            this.Name = dynamicObjectAttribute.Name ?? this.MappedType.FullName;
            this.artifactIdProperty = this.MappedType.GetProperty("ArtifactId");
        }

        public Type MappedType { get; private set; }

        public Guid TypeGuid { get; private set; }

        public string Name { get; private set; }

#pragma warning disable SA1201 // Elements must appear in the correct order
        private PropertyInfo artifactIdProperty;

        private Dictionary<Guid, PropertyInfo> fields = new Dictionary<Guid, PropertyInfo>();

#pragma warning restore SA1201 // Elements must appear in the correct order

        public object Map(DTOs.Artifact sourceDynamicObject)
        {
            var instance = Activator.CreateInstance(this.MappedType);
            foreach (var guidPropertyInfo in this.fields)
            {
                var guid = guidPropertyInfo.Key;
                var setter = guidPropertyInfo.Value.SetMethod;
                var value = sourceDynamicObject[guid].Value;

                setter.Invoke(instance, new object[] { value });
            }

            return instance;
        }

#pragma warning disable SA1204 // Static elements must appear before instance elements
        public static RDOMap MakeMap(Type mappedType)
#pragma warning restore SA1204 // Static elements must appear before instance elements
        {
            var dynamicObjectAttribute = mappedType.GetCustomAttribute<RelativityDynamicObjectAttribute>();
            if (dynamicObjectAttribute == null)
            {
                throw new RDOMappingException(mappedType, string.Format("{0} does not have a RelativityDynamicObjectAttribute", mappedType.Name));
            }

            var rdoMap = new RDOMap(mappedType, dynamicObjectAttribute);
            rdoMap.CreateMap();
            return rdoMap;
        }

        private void CreateMap()
        {
            var mappedProperties = this.MappedType.GetProperties()
            .Select(p => new Tuple<RelativityDynamicObjectAttribute, PropertyInfo>(p.GetCustomAttribute<RDOFieldAttribute>(), p))
            .Where(t => t.Item2.SetMethod != null && t.Item1 != null);

            foreach (var propertyTuple in mappedProperties)
            {
                var guid = propertyTuple.Item1.Guid;
                var propertyInfo = propertyTuple.Item2;
                if (this.fields.ContainsKey(propertyTuple.Item1.Guid))
                {
                    throw new DuplicateFieldException(this.MappedType, propertyInfo.Name, guid);
                }

                this.fields.Add(guid, propertyInfo);
            }
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class RDOMap<T> : RDOMap
#pragma warning restore SA1402 // File may only contain a single class
    {
        protected RDOMap(Type type, RelativityDynamicObjectAttribute dynamicObjectAttribute)
            : base(type, dynamicObjectAttribute)
        {
        }

        public new T Map(DTOs.Artifact sourceDynamicObject)
        {
            return (T)base.Map(sourceDynamicObject);
        }

#pragma warning disable SA1204 // Static elements must appear before instance elements
        public static RDOMap<T> MakeMap()
#pragma warning restore SA1204 // Static elements must appear before instance elements
        {
            return (RDOMap<T>)MakeMap(typeof(T));
        }
    }
}
