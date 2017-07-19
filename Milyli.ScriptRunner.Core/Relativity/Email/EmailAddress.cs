namespace Milyli.ScriptRunner.Core.Relativity.Email
{
    using System.Net.Mail;
    /// <summary>
    /// Email Address
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        /// Gets or sets email Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Returns a System.Net..Mail.MailAddess
        /// </summary>
        /// <returns>The To mail address.</returns>
        public MailAddress ToMailAddress()
        {
            return string.IsNullOrEmpty(this.DisplayName) ? new MailAddress(this.Address) : new MailAddress(this.Address, this.DisplayName);
        }
    }
}
