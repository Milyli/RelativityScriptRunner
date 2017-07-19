using Milyli.ScriptRunner.Core.Relativity.Configuration;

namespace Milyli.ScriptRunner.Core.Relativity.Email
{
    /// <inheritdoc/>
    public class EmailConfiguration
    {
        /// <summary>
        /// Gets or sets the email address that appears in the From field of email messages that contain authentication information for users
        /// </summary>
        public string AuthenticationEmailFrom { get; set; }

        /// <summary>
        /// Gets or sets the email address populated in the "From" field when sending email notifications.
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// Gets or sets the email address populated in the "To" field when sending email notifications.
        /// </summary>
        public string EmailTo { get; set; }

        /// <summary>
        /// Gets or sets the name of the environment. This value is used when sending notifications.
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// Gets or sets the password for the username associated with the credentials of the SMTP server.
        /// </summary>
        [ConfigurationEncrypted]
        public string SmtpPassword { get; set; }

        /// <summary>
        /// Gets or sets the port used for SMTP transactions.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the SMTP server used to send notifications.
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether smtp requires ssl or not
        /// </summary>
        public bool SmtpSslIsRequired { get; set; }

        /// <summary>
        /// Gets or sets the username associated with the credentials of the SMTP server.
        /// </summary>
        public string SmtpUserName { get; set; }
    }
}
