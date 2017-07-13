namespace Milyli.ScriptRunner.Core.Relativity.Email
{
    using System.Threading.Tasks;
    /// <summary>
    /// This service allows you to send emails using relativity instance settings values.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Explicitly loads the email configuration, completing all data access neccessary for sending mail.
        /// </summary>
        void LoadConfiguration();

        /// <summary>
        /// Sends a email.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A task that sends and email.</returns>
        Task SendAsync(EmailMessage message);
    }
}
