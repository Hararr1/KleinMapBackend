using KleinMapLibrary.Interfaces;
using System.Net;
using System.Net.Mail;

namespace KleinMapLibrary.Managers
{
    public class SMTPClient : ISMTPClient
    {
        // ------ PROPERTIES ------ //
        private readonly SmtpClient smtpClient;

        // ------ CTOR AND INIT METHOD ------ //
        public SMTPClient()
        {
            smtpClient = Initialize();
        }

        private SmtpClient Initialize()
        {
            return new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("2c30466eeed8ba", "9ede35870a7f8b"),
                EnableSsl = true,
            };
        }

        // ------ PUBLIC METHODS ------ //
        public void SendMail(string from, string reciver, string subject, string body)
        {
            smtpClient.Send(new MailMessage(from, reciver, subject, body) { IsBodyHtml = true });
        }
    }
}
