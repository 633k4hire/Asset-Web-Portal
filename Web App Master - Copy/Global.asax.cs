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

namespace Web_App_Master
{
   
    public class Global : HttpApplication
    {
        public static AssetLibrary _Library;
        public static XmlDocument _XmlLibrary;
        public static XmlDocument _Settings;
        void Application_Start(object sender, EventArgs e)
        {
            _Library = new AssetLibrary();
            _Settings = new XmlDocument();
            _Settings.Load(Server.MapPath("~/App_Data/settings.xml"));
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}