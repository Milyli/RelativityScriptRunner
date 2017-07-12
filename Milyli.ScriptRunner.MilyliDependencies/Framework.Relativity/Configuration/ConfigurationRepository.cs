// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using LinqToDB.Data;
    using Repositories.Interfaces;
    public class ConfigurationRepository<T> : IConfigurationRepository<T>
        where T : new()
    {
        private Framework.Repositories.DataContext dataContext;
        private IConfigurationCryptoService cryptoService;
        private bool isDisposed = false;
        private List<PropertyInfo> properties = null;
        private string sectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRepository{T}"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="cryptoService">The cryptography service with which to encrypt or decrypt values.</param>
        public ConfigurationRepository(Framework.Repositories.DataContext dataContext, IConfigurationCryptoService cryptoService)
        {
            this.dataContext = dataContext;
            this.cryptoService = cryptoService;
            this.properties = typeof(T).GetProperties().ToList();
            this.sectionName = typeof(T).GetCustomAttribute<ConfigurationSectionAttribute>().ConfigurationSection;
        }

        /// <inheritdoc/>
        public T ReadConfiguration()
        {
            var configTable = this.ReadSectionFromConfigTable();
            var propertyValues = configTable.Rows.Cast<DataRow>().Join(this.properties, r => r[0].ToString().ToUpperInvariant(), p => p.Name.ToUpperInvariant(), (r, p) => new
            {
                Property = p,
                Value = r[1]
            });
            var retval = new T();
            foreach (var pv in propertyValues)
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(pv.Property.PropertyType);
                try
                {
                    string stringValue = (string)pv.Value;
                    if (Attribute.IsDefined(pv.Property, typeof(ConfigurationEncryptedAttribute)))
                    {
                        try
                        {
                            stringValue = this.cryptoService.Decrypt(stringValue);
                        }
                        catch (FormatException)
                        {
                            // If the value in the DB is base 64 encoded, replace it with the encrypted value on reads
                            this.Update(pv.Property.Name, stringValue, true);
                        }
                        catch (System.Security.Cryptography.CryptographicException)
                        {
                            // If the value in the DB is not encrypted, replace it with the encrypted value on reads
                            this.Update(pv.Property.Name, stringValue, true);
                        }
                    }

                    pv.Property.SetValue(retval, converter.ConvertFromString(stringValue));
                }
                catch (FormatException)
                {
                } // leave the property with it's default value if the data doesn't parse
            }

            return retval;
        }

        /// <inheritdoc/>
        public void SetConfiguration(T configuration)
        {
            foreach (var p in this.properties)
            {
                this.Update(p.Name, p.GetValue(configuration).ToString(), Attribute.IsDefined(p, typeof(ConfigurationEncryptedAttribute)));
            }
        }

        /// <summary>
        /// Disposal method for Disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);

            // Uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up resources when the class leaves scope.
        /// </summary>
        /// <param name="isDisposing">Indicates whether the calling method is this class' Dispose method.</param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    // Dispose managed state (managed objects).
                    this.dataContext.Dispose();
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.
                // Set large fields to null.
                this.isDisposed = true;
            }
        }

        private DataTable ReadSectionFromConfigTable()
        {
            const string sql = @"SELECT Name, Value FROM [eddsdbo].[Configuration] WHERE Section=@sectionName";
            var configTable = new DataTable { Locale = System.Globalization.CultureInfo.InvariantCulture };
            if (string.IsNullOrEmpty(this.sectionName))
            {
                throw new ArgumentException("Invalid section name.");
            }

            configTable.Load(this.dataContext.ExecuteReader(sql, DataParameter.NVarChar("sectionName", this.sectionName)).Reader);
            return configTable;
        }

        private void Update(string name, string newValue, bool encrypt)
        {
            if (encrypt)
            {
                newValue = this.cryptoService.Encrypt(newValue);
            }

            const string Sql = @"UPDATE [eddsdbo].[Configuration] SET value = @value WHERE [Section]=@section AND [Name] = @name";
            var parameters = new { name = name, section = this.sectionName, value = newValue };
            this.dataContext.Execute(Sql, parameters);
        }

        private string GetString(string name)
        {
            return this.GetString(name, this.sectionName);
        }

        private string GetString(string name, string section)
        {
            const string Sql = @"SELECT Value FROM [eddsdbo].[Configuration] WHERE [Section]={1} AND [Name] = {0}";
            object[] parameters = { name, section };
            var value = this.dataContext.Query<string>(Sql, parameters).FirstOrDefault();
            return value;
        }

        private int GetInt(string name, int defaultValue, int lowerBound = 1, int upperBound = int.MaxValue)
        {
            var strValue = this.GetString(name);
            int value;
            bool parsedSuccessfully = int.TryParse(strValue, out value);
            if (!parsedSuccessfully || value < lowerBound || value > upperBound)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        private bool GetBool(string name, bool defaultValue = false)
        {
            var strValue = this.GetString(name);
            bool value;
            bool parsedSuccessfully = bool.TryParse(strValue, out value);
            if (!parsedSuccessfully)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }
    }
}
