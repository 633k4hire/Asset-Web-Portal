using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using Web_App_Master.Account;
using Notification;
using static Notification.NotificationSystem;

namespace Web_App_Master
{
   
    public class Global : HttpApplication
    {

        public static NotificationSystem NoticeSystem;
        public static AssetLibrary Library;
        public static UPSaccount _UPSAccount
        {
            get
            {
                return Global.Library.Settings.UpsAccount;
            }
            set
            {
                Global.Library.Settings.UpsAccount = value;
            }
        }
        void Application_Start(object sender, EventArgs e)
        {           
            LoadLibrary();
            LoadSettings();
            LoadNotificationSystem();
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session["SessionUserData"] = new Data.UserData();            
        }
        public static void LoadLibrary()
        {
            Library = new AssetLibrary();
            try
            {
                Load.Library();
            }
            catch { }
        }
        private static void LoadSettings()
        {
            Library.Settings = new Settings();
            try
            {
                Load.Globals();
                if(Library.Settings==null)
                {
                    Library.Settings = new Settings();
                }
            }
            catch { }
        }
        private static void LoadNotificationSystem()
        {
            //Loads and Starts Notification System
            NoticeSystem = new NotificationSystem();
            try
            {
                Load.NotificationSystem();
                if (NoticeSystem == null)
                {
                    NoticeSystem = new NotificationSystem("AWP_Notification_System");
                }
            }
            catch { }
            if (Global.Library.Settings.TESTMODE)
            {
                NoticeSystem.Interval = 60000;
                NoticeSystem.OnTimerTick += NoticeSystem_OnTimerTick;
            }
            
            NoticeSystem.OnNoticeAdded += NoticeSystem_OnNoticeAdded;
            NoticeSystem.OnNoticeChanged += NoticeSystem_OnNoticeChanged;
            NoticeSystem.OnScheduledTime += NoticeSystem_OnNoticeExpired;
            NoticeSystem.OnNoticeRemoved += NoticeSystem_OnNoticeRemoved;
            
        }

        private static void NoticeSystem_OnTimerTick(object sender, NotificationSystem.TimerTickEvent e)
        {
            
        }

        private static void NoticeSystem_OnNoticeRemoved(object sender, NotificationSystem.NotificationEvent e)
        {
            
        }

        private static void NoticeSystem_OnNoticeExpired(object sender, NotificationSystem.NotificationEvent e)
        {
          
        }

        private static void NoticeSystem_OnNoticeChanged(object sender, NotificationSystem.NotificationEvent e)
        {
          
        }

        private static void NoticeSystem_OnNoticeAdded(object sender, NotificationSystem.NotificationEvent e)
        {
          
        }
        public static Notice CheckoutAction(Notice n)
        {

            return n;
        }
    }
}