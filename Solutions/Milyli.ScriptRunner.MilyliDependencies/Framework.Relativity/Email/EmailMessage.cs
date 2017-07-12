// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Email
{
    using System.Collections.Generic;

    /// <summary>
    /// Email model
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        public EmailMessage()
        {
            this.ToAddresses = new List<EmailAddress>();
            this.CCAddresses = new List<EmailAddress>();
            this.BccAddresses = new List<EmailAddress>();
        }

        /// <summary>
        /// Gets or sets the body of a email
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a EmailAddress that contains an e-mail address.
        /// </summary>
        public EmailAddress FromAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether of the body contains HTML
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Gets or sets a System.String that contains an e-mail address.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets a list of EmailAddresses that contains zero or more e-mail addresses to which the email will be sent.
        /// </summary>
        public List<EmailAddress> ToAddresses { get; set; }

        /// <summary>
        /// Gets or sets a list of EmailAddresses that contains zero or more e-mail addresses to which the email will be carbon copied.
        /// </summary>
        public List<EmailAddress> CCAddresses { get; set; }

        /// <summary>
        /// Gets or sets a list of EmailAddresses that contains zero or more e-mail addresses to which the email will be blind carbon copied.
        /// </summary>
        public List<EmailAddress> BccAddresses { get; set; }
    }
}
