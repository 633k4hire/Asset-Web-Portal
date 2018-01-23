using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using Web_App_Master;
using Web_App_Master.Models;

namespace Helpers
{
    public static class Extensions
    {
        public static void Shake(this HtmlGenericControl control, string animation= " mif-ani-flash")
        {
            var a = control.Attributes["class"];
            a += animation;
            control.Attributes["class"] = a;
        }
        public static void Quiet(this HtmlGenericControl control, string animation = " mif-ani-flash")
        {
            var a = control.Attributes["class"];
            a = a.Replace(animation, "");
            control.Attributes["class"] = a;
        }

        public static SiteMaster SiteMaster(this Page page)
        {
            return page.Master as SiteMaster;
        }
        public static void UpdateAll(this Page page)
        {
            (page.Master as SiteMaster).UpdateAllPanels();
        }

        public static List<Asset> ImportXmlLibraryFile(string filename)
        {

            var doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(filename))
            {
                doc.LoadXml(reader.ReadToEnd());
            }

        
            Global.Library.Assets = new List<Asset>();

            XmlNodeList elemList = doc.GetElementsByTagName("Asset");
            foreach (XmlElement asset in elemList)
            {
                Asset a = new Asset();
                try
                {

                    a.AssetName = asset.GetAttribute("Name").Sanitize();
                }
                catch { }
                try
                {
                    a.AssetNumber = asset.SelectSingleNode("AssetNumber").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.DateRecieved = DateTime.Parse(asset.SelectSingleNode("DateRecieved").InnerText.Sanitize());
                }
                catch { }
                try
                {
                    a.DateShipped = DateTime.Parse(asset.SelectSingleNode("DateShipped").InnerText.Sanitize());
                }
                catch { }
                try
                {
                    a.Description = asset.SelectSingleNode("Description").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.OrderNumber = asset.SelectSingleNode("OrderNumber").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.PackingSlip = asset.SelectSingleNode("PackingSlip").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.PersonShipping = asset.SelectSingleNode("PersonShipping").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.ReturnReport = asset.SelectSingleNode("ReturnReport").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.ServiceEngineer = asset.SelectSingleNode("ServiceEngineer").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.ShipTo = asset.SelectSingleNode("ShipTo").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.UpsLabel = asset.SelectSingleNode("UpsLabel").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.weight = Convert.ToDecimal(asset.SelectSingleNode("Weight").InnerText.Sanitize());
                }
                catch { }
                try
                {
                    a.IsOut = Convert.ToBoolean(asset.SelectSingleNode("IsOut").InnerText.Sanitize());
                }
                catch { }
                try
                {
                    a.OnHold = Convert.ToBoolean(asset.SelectSingleNode("OnHold").InnerText.Sanitize());
                }
                catch { }
                try
                {
                    a.IsDamaged = Convert.ToBoolean(asset.SelectSingleNode("IsDamaged").InnerText.Sanitize());
                }
                catch { }


                try
                {
                    XmlNodeList imglist = asset.GetElementsByTagName("Image");

                    foreach (XmlElement el in imglist)
                    {
                        a.Images += "" + el.InnerText + ",";
                    }
                }
                catch { }

                Global.Library.Assets.Add(a);

            }

            return Global.Library.Assets;
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

        }

        public static string Sanitize(this string str)
        {
            if (str == null)
                str = "";
            return str.Replace("\"", "").Replace("'", "");
        }
        public static string SanitizeHTML(this string str)
        {
            if (str == null)
                str = "";
            return str.Replace("<", "").Replace(">", "").Replace("*", "").Replace(":", "").Replace("|", "");
        }
        public static Asset FindAssetByNumber(this List<Asset> assets, string assetNumber)
        {
            return (from x in assets
                    where x.AssetNumber == assetNumber
                    select x).FirstOrDefault();
        }
        public static Asset FindAssetByName(this List<Asset> assets, string assetName)
        {
            return (from x in assets
                    where x.AssetName == assetName
                    select x).FirstOrDefault();
        }
        public static List<Asset> FindAssetsByNumber(this List<Asset> assets, string assetNumber)
        {
            return (from x in assets
                    where x.AssetNumber == assetNumber
                    select x).ToList();
        }

        public static RoleBindinglist ToRoleBindingList(this IEnumerable<IdentityRole> list)
        {
            return new RoleBindinglist(list.ToList());
        }
        public static UserBindinglist ToUserBindingList(this IEnumerable<ApplicationUser> list)
        {
            return new UserBindinglist(list.ToList());
        }
        public static AssetNoticeBindinglist ToNoticeBindingList(this IEnumerable<AssetNotification> list)
        {
            return new AssetNoticeBindinglist(list.ToList());
        }
        public static Asset ToAsset(this XmlElement asset)
        {
            Asset a = new Asset();
            try
            {

                a.AssetName = asset.GetAttribute("Name").Sanitize();
            }
            catch { }
            try
            {
                a.AssetNumber = asset.SelectSingleNode("AssetNumber").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.DateRecieved = DateTime.Parse(asset.SelectSingleNode("DateRecieved").InnerText.Sanitize());
            }
            catch { }
            try
            {
                a.DateShipped = DateTime.Parse(asset.SelectSingleNode("DateShipped").InnerText.Sanitize());
            }
            catch { }
            try
            {
                a.Description = asset.SelectSingleNode("Description").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.OrderNumber = asset.SelectSingleNode("OrderNumber").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.PackingSlip = asset.SelectSingleNode("PackingSlip").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.PersonShipping = asset.SelectSingleNode("PersonShipping").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.ReturnReport = asset.SelectSingleNode("ReturnReport").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.ServiceEngineer = asset.SelectSingleNode("ServiceEngineer").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.ShipTo = asset.SelectSingleNode("ShipTo").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.UpsLabel = asset.SelectSingleNode("UpsLabel").InnerText.Sanitize();
            }
            catch { }
            try
            {
                a.weight = Convert.ToDecimal(asset.SelectSingleNode("Weight").InnerText.Sanitize());
            }
            catch { }
            try
            {
                a.IsOut = Convert.ToBoolean(asset.SelectSingleNode("IsOut").InnerText.Sanitize());
            }
            catch { }
            try
            {
                a.OnHold = Convert.ToBoolean(asset.SelectSingleNode("OnHold").InnerText.Sanitize());
            }
            catch { }
            try
            {
                a.IsDamaged = Convert.ToBoolean(asset.SelectSingleNode("IsDamaged").InnerText.Sanitize());
            }
            catch { }
            try
            {
                XmlNodeList imglist = asset.GetElementsByTagName("Image");

                foreach (XmlElement el in imglist)
                {
                    a.Images += "" + el.InnerText + ",";
                }
            }
            catch { }
            return a;
        }
        public static AssetNotification ToNotification(this XmlElement asset)
        {
            AssetNotification a = new AssetNotification();
            try
            {
                a.AssetNumber = asset.GetAttribute("AssetNumber").Sanitize();
                a.IsNotified = Convert.ToBoolean(asset.GetAttribute("IsNotified").Sanitize());
                a.LastNotified = DateTime.Parse(asset.GetAttribute("LastNotified").Sanitize());
                a.Time = DateTime.Parse(asset.GetAttribute("Time").Sanitize());
                var n30 = asset.GetAttribute("Is30day").Sanitize();
                var n15 = asset.GetAttribute("Is15Day").Sanitize();
                a.Is30Day = Convert.ToBoolean(n30);
                a.Is15Day = Convert.ToBoolean(n15);
                XmlNodeList elemList = asset.GetElementsByTagName("Email");
                foreach (XmlElement elem in elemList)
                {
                    EmailAddress em = new EmailAddress();
                    em.Email = asset.GetAttribute("Email").Sanitize();
                    em.Name = asset.GetAttribute("Name").Sanitize();
                    a.Emails.Add(em);
                }
            }
            catch { }


            return a;
        }
        public static CustomerBindinglist ToCustomerBindingList(this IEnumerable<Customer> list)
        {
            return new CustomerBindinglist(list.ToList());

        }
    }

}
namespace System.Collections.Generic
{
    public class SizedList<T> : ConcurrentQueue<T>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }

        public SizedList(int size)
        {
            Size = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    T outObj;
                    base.TryDequeue(out outObj);
                }
            }
        }
    }
}