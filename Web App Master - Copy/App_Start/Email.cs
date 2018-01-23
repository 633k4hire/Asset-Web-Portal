using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace EmailHelper
{
    public static class EmailHelper
    {
        public static void SendEmailNoticeAsync(string email,string body = "", string subject = "")
        {


            new System.Threading.Thread(() =>
            {

                MailMessage msg = new MailMessage();
                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.ASCII;
                msg.Body = body;
                msg.From = new MailAddress("provider.service.secure@gmail.com");
                msg.To.Add(email);
                string username = "provider.service.secure@gmail.com";  //email address or domain user for exchange authentication
                string password = "@Service1";  //password
                SmtpClient mClient = new SmtpClient();
                mClient.Host = "smtp.gmail.com";
                mClient.Port = 587;
                mClient.UseDefaultCredentials = false;
                mClient.Credentials = new NetworkCredential(username, password);
                mClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mClient.Timeout = 100000;
                mClient.EnableSsl = true;
                try
                {
                    mClient.Send(msg);
                }
                catch
                {
                 
                }
            }).Start();
        }
        public static void SendMassEmailNoticeAsync(string[] emails, string body = "", string subject = "")
        {


            new System.Threading.Thread(() =>
            {

                MailMessage msg = new MailMessage();
                msg.Subject = subject;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.ASCII;
                msg.Body = body;
                msg.From = new MailAddress("provider.service.secure@gmail.com");
                foreach(var email in emails )
                {
                    msg.To.Add(email);
                }                
                string username = "provider.service.secure@gmail.com";  //email address or domain user for exchange authentication
                string password = "@Service1";  //password
                SmtpClient mClient = new SmtpClient();
                mClient.Host = "smtp.gmail.com";
                mClient.Port = 587;
                mClient.UseDefaultCredentials = false;
                mClient.Credentials = new NetworkCredential(username, password);
                mClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mClient.Timeout = 100000;
                mClient.EnableSsl = true;
                try
                {
                    mClient.Send(msg);
                }
                catch
                {

                }
            }).Start();
        }

    }
}