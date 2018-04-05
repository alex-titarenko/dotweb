using System;
using System.Net.Mail;
using System.Text;


namespace TAlex.Web.Services.Mail
{
    /// <summary>
    /// Represents the interface for all classes that implement functionality to send e-mail to the mail server.
    /// </summary>
    public interface IMailService : IDisposable
    {
        /// <summary>
        /// Sends the specified message to an mail server for delivery.
        /// </summary>
        /// <param name="message">A <see cref="System.Net.Mail.MailMessage"/> that contains the message to send.</param>
        void Send(MailMessage message);

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        void Send(string recipients, string subject, string body);

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="from">A <see cref="System.String"/> that contains the address information of the message sender.</param>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        void Send(string from, string recipients, string subject, string body);

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="from">A <see cref="System.String"/> that contains the address information of the message sender.</param>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        /// <param name="IsBodyHtml">A value indicating whether the mail message body is in Html.</param>
        void Send(string from, string recipients, string subject, string body, bool IsBodyHtml);

        /// <summary>
        /// Sends the specified e-mail message to an mail server for delivery.
        /// </summary>
        /// <param name="from">A <see cref="System.String"/> that contains the address information of the message sender.</param>
        /// <param name="recipients">A <see cref="System.String"/> that contains the addresses that the message is sent to.</param>
        /// <param name="subject">A <see cref="System.String"/> that contains the subject line for the message.</param>
        /// <param name="body">A <see cref="System.String"/> that contains the message body.</param>
        /// <param name="IsBodyHtml">A value indicating whether the mail message body is in Html.</param>
        /// <param name="encoding">A <see cref="System.Text.Encoding"/> used to encode the message body.</param>
        void Send(string from, string recipients, string subject, string body, bool IsBodyHtml, Encoding encoding);
    }
}
