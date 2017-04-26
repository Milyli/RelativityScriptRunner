#pragma warning disable SA1402 // File may only contain a single class
namespace Milyli.ScriptRunner.Data.Relativity.Mapping
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using kCura.Relativity.Client;
    using DTOs = kCura.Relativity.Client.DTOs;

    public class DynamicObjectMapper
    {
        private static Dictionary<Type, RDOMap> maps = new Dictionary<Type, RDOMap>();

        public static object PopulateArtifactFields(Type destinationType, DTOs.RDO artifact)
        {
            var result = Activator.CreateInstance(destinationType);
            var props = destinationType.GetProperties()
                .Select(p => new Tuple<MethodInfo, RelativityDynamicObjectAttribute>(p.GetSetMethod(), p.GetCustomAttribute<RDOFieldAttribute>()))
                .Where(t => t.Item1 != null && t.Item2 != null);
            var artifactId = destinationType.GetProperty("ArtifactId");
            artifactId?.SetMethod.Invoke(result, new object[] { artifact.ArtifactID });
            foreach (var prop in props)
            {
                var guid = prop.Item2.Guid;
                var value = artifact[guid]?.Value;
                var setter = prop.Item1;
                setter.Invoke(result, new[] { value });
            }

            return result;
        }

        public static T Get<T>(DTOs.RDO rdo, IRSAPIClient retreivalClient)
        {
            var attr = typeof(T).GetCustomAttribute<RelativityDynamicObjectAttribute>();
            var instance = PopulateArtifactFields(typeof(T), rdo);
            PopulateCollections(instance, typeof(T), rdo, retreivalClient);
            PopulateObjects(instance, typeof(T), rdo, retreivalClient);
            if (attr != null)
            {
                return (T)instance;
            }

            return default(T);
        }

        public static ObjectCondition MakeCondition<T>(string propertyName, ObjectConditionEnum compare, int value)
        {
            var property = typeof(T).GetProperty(propertyName);
            var attr = property.GetCustomAttribute<RelativityDynamicObjectAttribute>();
            return new ObjectCondition(attr.Guid, compare, value);
        }

        public static List<T> Query<T>(IRSAPIClient retreivalClient, Condition condition)
        {
            var attr = typeof(T).GetCustomAttribute<RelativityDynamicObjectAttribute>();
            var collection = new List<T>();
            var query = new DTOs.Query<DTOs.RDO>()
            {
                ArtifactTypeGuid = attr.Guid,
                Condition = condition,
                Fields = DTOs.FieldValue.AllFields
            };

            var results = retreivalClient.Repositories.RDO.Query(query);
            if (results.Success)
            {
                foreach (var result in results.Results)
                {
                    var item = (T)PopulateArtifactFields(typeof(T), result.Artifact);
                    collection.Add(item);
                }
            }

            return collection;
        }

        private static IList GetCollection(Type listType, DTOs.FieldValueList<DTOs.Artifact> artifacts, IRSAPIClient proxy)
        {
            var artifactType = listType.GetGenericArguments().First();
            var collectionResult = (IList)Activator.CreateInstance(listType, null);
            var subRdo = proxy.Repositories.RDO.Read(artifacts.Select(a => a.ArtifactID).ToList());
            if (subRdo.Success)
            {
                foreach (var res in subRdo.Results)
                {
                    collectionResult.Add(PopulateArtifactFields(artifactType, res.Artifact));
                }
            }

            return collectionResult;
        }

        private static void PopulateCollections(object instance, Type type, DTOs.RDO rdo, IRSAPIClient proxy)
        {
            var collections = type.GetProperties()
                .Select(p => new Tuple<PropertyInfo, RelativityDynamicObjectAttribute>(p, p.GetCustomAttribute<RDOCollectionAttribute>()))
                .Where(t => t.Item1 != null && t.Item2 != null);
            foreach (var collection in collections)
            {
                var collectionType = collection.Item1.PropertyType;
                var collectionGuid = collection.Item2.Guid;
                var artifacts = rdo[collectionGuid].GetValueAsMultipleObject<DTOs.Artifact>();
                var result = GetCollection(collectionType, artifacts, proxy);
                collection.Item1.SetMethod?.Invoke(instance, new[] { result });
            }
        }

        private static void PopulateObjects(object instance, Type type, DTOs.RDO rdo, IRSAPIClient retrievalClient)
        {
            var objects = type.GetProperties()
                .Select(p => new Tuple<PropertyInfo, RelativityDynamicObjectAttribute>(p, p.GetCustomAttribute<RDOSingleObjectAttribute>()))
                .Where(t => t.Item1 != null && t.Item2 != null && t.Item1.SetMethod != null);

            foreach (var obj in objects)
            {
                var destinationType = obj.Item1.PropertyType;
                var guid = obj.Item2.Guid;
                var objectRdo = rdo[guid].ValueAsSingleObject;
                var destinationRdo = retrievalClient.Repositories.RDO.ReadSingle(objectRdo.ArtifactID);
                var result = PopulateArtifactFields(destinationType, destinationRdo);
                obj.Item1.SetMethod.Invoke(instance, new[] { result });
            }
        }
    }

    public abstract class ArtifactBacked
    {
        public int ArtifactId { get; set; }
    }
}
#pragma warning restore SA1402 // File may only contain a single class
