using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Web_App_Master;

namespace Web_App_Master
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
        public static void SendNoticeAsync(List<Asset> assets, string Body = "")
        {
            var engineer = (from d in Global.Library.Settings.ServiceEngineers where d.Name == assets[0].ServiceEngineer select d).FirstOrDefault();
            var statics = (from d in Global.Library.Settings.StaticEmails  select d.Email).ToList();
       

            var emaillist = new List<string>();
            //emaillist.Add(shipper.Email);
            emaillist.Add(engineer.Email);
            emaillist.AddRange(statics);
            
            new System.Threading.Thread(() =>
            {

                Body = Body.Replace("<name>", assets[0].ServiceEngineer);
                var serviceToolString = " < br />";
                foreach(var item in assets)
                {
                    serviceToolString += item.AssetName + "< br />";
                }
                
                Body = Body.Replace("<serviceTool>", serviceToolString);
                Body = Body.Replace("<serviceOrder>", assets[0].OrderNumber.ToString());
                Body = Body.Replace("<dateAssigned>", assets[0].DateShipped.ToString());
                Body = Body.Replace("<NL>", "<br />");
                MailMessage msg = new MailMessage();
                msg.Subject = "Asset Alert:: " + assets[0].AssetName + " :: " + DateTime.Now.ToString();
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.ASCII;
                msg.Body = Body;
                msg.From = new MailAddress("provider.service.secure@gmail.com");
                foreach (var email in emaillist)
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