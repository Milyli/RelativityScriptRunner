namespace Milyli.ScriptRunner.Core.Repositories.Interfaces
{
    /// <summary>
    /// Interface that defines configuration security utilities.
    /// </summary>
    public interface IConfigurationCryptoService
    {
        /// <summary>
        /// Encrypts a string of data.
        /// </summary>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>A string containing the encrypted data.</returns>
        string Encrypt(string data);

        /// <summary>
        /// Decrypts a string of data.
        /// </summary>
        /// <param name="encryptedData">The data to be decrypted</param>
        /// <returns>A string containing the decrypted data.</returns>
        string Decrypt(string encryptedData);
    }
}
