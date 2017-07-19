namespace Milyli.ScriptRunner.Core.Relativity.Email
{
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Repositories.Interfaces;

    /// <inheritdoc/>
    public class EmailService : IEmailService
    {
        private readonly IConfigurationRepository<EmailConfiguration> configurationRepository;
        private EmailConfiguration configuration;

        /// <summary>
        ///  Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="configurationRepository">The source of the email configuration.</param>
        public EmailService(IConfigurationRepository<EmailConfiguration> configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        /// <inheritdoc/>
        public void LoadConfiguration()
        {
            this.configuration = this.configurationRepository.ReadConfiguration();
        }

        /// <inheritdoc/>
        public async Task SendAsync(EmailMessage message)
        {
            this.configuration = this.configuration ?? this.configurationRepository.ReadConfiguration();
            using (var client = GetSmtpClient(this.configuration))
            {
                using (var mail = GetMailMessage(message))
                {
                    await client.SendMailAsync(mail);
                }
            }
        }

        private static SmtpClient GetSmtpClient(EmailConfiguration configuration)
        {
            var client = new SmtpClient(configuration.SmtpServer);
            if (string.IsNullOrEmpty(configuration.SmtpUserName) && string.IsNullOrEmpty(configuration.SmtpPassword))
            {
                client.UseDefaultCredentials = true;
            }
            else
            {
                client.Credentials = new NetworkCredential(configuration.SmtpUserName, configuration.SmtpPassword);
            }

            client.Port = configuration.SmtpPort;
            client.EnableSsl = configuration.SmtpSslIsRequired;

            return client;
        }

        private static MailMessage GetMailMessage(EmailMessage message)
        {
            var mail = new MailMessage
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml,
                From = message.FromAddress.ToMailAddress(),
            };
            message.ToAddresses.ForEach(x => mail.To.Add(x.ToMailAddress()));
            message.CCAddresses.ForEach(x => mail.CC.Add(x.ToMailAddress()));
            message.BccAddresses.ForEach(x => mail.Bcc.Add(x.ToMailAddress()));
            return mail;
        }
    }
}
