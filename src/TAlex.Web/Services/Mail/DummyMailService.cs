using System.Net.Mail;
using System.Text;


namespace TAlex.Web.Services.Mail
{
    public class DummyMailService : IMailService
    {
        public void Dispose()
        {
        }

        public void Send(MailMessage message)
        {
        }

        public void Send(string recipients, string subject, string body)
        {
        }

        public void Send(string from, string recipients, string subject, string body)
        {
        }

        public void Send(string from, string recipients, string subject, string body, bool IsBodyHtml)
        {
        }

        public void Send(string from, string recipients, string subject, string body, bool IsBodyHtml, Encoding encoding)
        {
        }
    }
}
