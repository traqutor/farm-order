using FarmOrder.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace FarmOrder.Utils
{
    public class EmailSender
    {
        private static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        private const string PasswordRecoveryPath = "app/passwordRecovery";

        public static bool SendEmail(string emailTo, string subject, string body)
        {
            var result = "";

            MailMessage message = new MailMessage(Settings.Default.SMTP_email, emailTo, subject, body);
            message.IsBodyHtml = true;

            try
            {
                SmtpClient smtp = new SmtpClient();
                //smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                smtp.Host = Settings.Default.SMTP_host;
                smtp.TargetName = $"STARTTLS/{Settings.Default.SMTP_host}";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Settings.Default.SMTP_email, Settings.Default.SMTP_password);
                smtp.Port = 587;
                smtp.Send(message);

                result = "";
            }
            catch (Exception ex)
            {
                LOGGER.ErrorException("Email sending email error", ex);
                result = ex.Message;
                return false;
            }

            //message.Dispose();
            return true;
        }

        public static bool SendPasswordRecoveryEmail(string emailTo, string subject, string callbackUrl, string token)
        {
            string body = $"Hello," +
                          $"<br/><br/>" +
                          $"Please click <a href='http://{callbackUrl}/{PasswordRecoveryPath}?token={token}&email={emailTo}'>here</a> to reset your password." +
                          $"<br/><br/>" +
                          $"Regards," +
                          $"<br/><br/>" +
                          $"Genius Support";

            return SendEmail(emailTo, subject, body);
        }
    }
}