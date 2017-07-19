namespace Milyli.ScriptRunner.Core.Relativity.Configuration
{
    using kCura.Config;
    using Repositories.Interfaces;
    /// <inheritdoc/>
    public class ConfigurationCryptoService : IConfigurationCryptoService
    {
        /// <inheritdoc/>
        public string Encrypt(string data)
        {
            return Manager.Encrypt(data);
        }

        /// <inheritdoc/>
        public string Decrypt(string encryptedData)
        {
            return Manager.Decrypt(encryptedData);
        }
    }
}
