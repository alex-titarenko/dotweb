using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace TAlex.Web.Services.Mail
{
    /// <summary>
    /// Allows applications to send e-mail by using the Simple Mail Transfer Protocol (SMTP).
    /// </summary>
    public class MailService : IMailService
    {
        /// <summary>
        /// Gets a value that indicates a base class for sending email messages to SMTP server.
        /// </summary>
        public SmtpClient Client { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.Common.Net.Mail.MailService"/> class by using
        /// configuration file settings.
        /// </summary>
        public MailService()
        {
            Client = new SmtpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.Common.Net.Mail.MailService"/> class that sends
        /// e-mail by using the specified SMTP server.
        /// </summary>
        /// <param name="host">A <see cref="System.String"/> that contains the name or IP address of the host used for SMTP transactions.</param>
        public MailService(string host)
            : this()
        {
            Client.Host = host;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.Common.Net.Mail.MailService"/> class that sends
        /// e-mail by using the specified SMTP server and port.
        /// </summary>
        /// <param name="host">A <see cref="System.String"/> that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An <see cref="System.Int32"/> greater than zero that contains the port to be used on host.</param>
        public MailService(string host, int port)
            : this(host)
        {
            Client.Port = port;
        }

        public MailService(string host, int port, string userName, string password)
        {
            Client = new SmtpClient(host, port)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(userName, password)
            };
        }


        #region IMailService Members

        /// <summary>
        /// Sends the specified message to an mail server for delivery.
        /// </summary>
        /// <param name="message">A <see cref="System.Net.Mail.MailMessage"/> that contains the message to send.</param>
        public void Send(MailMessage message)
        {
            Client.Send(message);
        }

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        public void Send(string recipients, string subject, string body)
        {
            Send(String.Empty, recipients, subject, body, false);
        }

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="from">A <see cref="System.String"/> that contains the address information of the message sender.</param>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        public void Send(string from, string recipients, string subject, string body)
        {
            Send(from, recipients, subject, body, false);
        }

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="from">A <see cref="System.String"/> that contains the address information of the message sender.</param>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        /// <param name="IsBodyHtml">A value indicating whether the mail message body is in Html.</param>
        public void Send(string from, string recipients, string subject, string body, bool IsBodyHtml)
        {
            Send(from, recipients, subject, body, IsBodyHtml, Encoding.UTF8);
        }

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="from">A <see cref="System.String"/> that contains the address information of the message sender.</param>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        /// <param name="IsBodyHtml">A value indicating whether the mail message body is in Html.</param>
        /// <param name="encoding">A <see cref="System.Text.Encoding"/> used to encode the message body.</param>
        public void Send(string from, string recipients, string subject, string body, bool IsBodyHtml, Encoding encoding)
        {
            MailMessage message = new MailMessage();
            message.To.Add(recipients);
            if (!String.IsNullOrEmpty(from))
            {
                message.From = new MailAddress(from);
            }
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = IsBodyHtml;
            message.BodyEncoding = encoding;

            if (IsBodyHtml)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, encoding, "text/html");
                message.AlternateViews.Add(htmlView);
            }

            Send(message);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Client != null)
            {
                //TODO: Insert code for disposing SmtpClient
            }
        }

        #endregion
    }
}
