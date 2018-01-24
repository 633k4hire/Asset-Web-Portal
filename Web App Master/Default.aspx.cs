using Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_App_Master.Account;
using Web_App_Master.Models;
using static Notification.NotificationSystem;

namespace Web_App_Master
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void testbtn_Click(object sender, EventArgs e)
        {
            var a = AssetController.GetAsset("0007");
            if(a.IsOut)
            {
                //Asset Is Out
                a.IsOut = false;
                AssetController.UpdateAsset(a);
                a = AssetController.GetAsset("0007");
            }
            else
            {
                //Asset Is Not Out
                a.IsOut = true;
                AssetController.UpdateAsset(a);
                a = AssetController.GetAsset("0007");
            }
        }

        protected void tester_Click(object sender, EventArgs e)
        {
            try
            {
                EmailNotice notice = new EmailNotice();

                notice.Scheduled = DateTime.Now.AddDays(30);
                notice.NoticeType = NoticeType.Checkout;
                notice.NoticeAction = Global.CheckoutAction;
                var assets = new List<Asset>() { Global.Library.Assets[0], Global.Library.Assets[1], Global.Library.Assets[2], Global.Library.Assets[3], Global.Library.Assets[4] };
                foreach (var ass in assets)
                {
                    notice.Assets.Add(ass.AssetNumber);
                }
                notice.NoticeControlNumber = assets[0].OrderNumber;               
                notice.Body = Global.Library.Settings.CheckOutMessage;
                notice.Subject = "Asset Return Reminder";
                var engineer = (from d in Global.Library.Settings.ServiceEngineers where d.Name == assets[0].ServiceEngineer select d).FirstOrDefault();
                var statics = (from d in Global.Library.Settings.StaticEmails select d).ToList();

                notice.Emails.Add(engineer);
                notice.Emails.AddRange(statics);
                notice.EmailAddress = engineer;
                Global.NoticeSystem.Add(notice);
                var a = Global.NoticeSystem.SerializeToXmlString(Global.NoticeSystem);
            }
            catch { }
        }
    }
}