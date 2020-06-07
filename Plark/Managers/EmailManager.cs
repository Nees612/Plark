using Plark.Managers.Interfaces;
using Plark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Plark.Managers
{
    public class EmailManager : IEmailManager
    {
        private static readonly string VERIFICATION_URL = "https://192.168.43.56:5001/Users/Verify";
        private static readonly string SOURCE_EMAIL = "plarkproject@gmail.com";
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void SendVerificationEmail(string email, string emailToken)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(SOURCE_EMAIL);
            mail.IsBodyHtml = true;
            mail.To.Add(email);
            mail.Subject = "Plark Email Verification";
            mail.Body = "<h3>Plark Email Verification</h3><br>Click <a href=" + VERIFICATION_URL + "/" + emailToken + ">here</a> to verify your email address.";
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(SOURCE_EMAIL, "Dorisz0802");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public long GenerateEmailId(long userId)
        {
            var random = new Random();
            var emailId = "";

            for (int i = 0; i < 8; i++)
            {

                emailId += (random.Next(0, 10).ToString());
            }
            emailId += userId.ToString();

            return long.Parse(emailId);
        }
    }
}
