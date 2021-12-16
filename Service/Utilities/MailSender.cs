using System;
using System.Net.Mail;
using DataModels.Models;

namespace Service.Utilities
{
    public class MailSender
    {
        public bool SendMail(MailSettings mailSettings)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = mailSettings.Host;
                smtp.Port = mailSettings.Port;
                smtp.Credentials = new System.Net.NetworkCredential(mailSettings.From, mailSettings.Password);
                smtp.EnableSsl = mailSettings.IsEnableSSL;

                MailMessage myMessage = new MailMessage();

                myMessage.To.Add(new MailAddress(mailSettings.To));
                myMessage.From = new MailAddress(mailSettings.From);
                myMessage.Body = mailSettings.Body;
                myMessage.IsBodyHtml = mailSettings.IsBodyHTML;

                if (!string.IsNullOrEmpty(mailSettings.CC))
                    myMessage.CC.Add(mailSettings.CC);

                myMessage.Subject = mailSettings.Subject;
                smtp.Send(myMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
