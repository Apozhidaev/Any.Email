using System.Net;
using System.Net.Mail;

namespace Any.Email.Models
{
    public class EmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(SmtpSettings settings)
        {
            _settings = settings;
        }

        public void Send(EmailModel email)
        {
            var smtp = new SmtpClient
            {
                Host = _settings.Host,
                Port = _settings.Port,
                EnableSsl = _settings.EnableSsl,
                UseDefaultCredentials = _settings.UseDefaultCredentials
            };

            if (!smtp.UseDefaultCredentials)
            {
                smtp.Credentials = new NetworkCredential(_settings.User, _settings.Password);
            }

            var mail = new MailMessage
            {
                From = new MailAddress(email.From),
                Subject = email.Subject,
                IsBodyHtml = true,
                Body = email.Body
            };

            string[] recipients = email.To.Split(',', ';');

            foreach (var to in recipients)
            {
                mail.To.Add(new MailAddress(to));
            }

            if (email.Attachmets != null)
            {
                foreach (string fileName in email.Attachmets)
                {
                    var attachment = new Attachment(fileName);
                    mail.Attachments.Add(attachment);
                }
            }

            smtp.Send(mail);
        }
    }
}