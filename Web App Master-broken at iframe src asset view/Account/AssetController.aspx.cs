using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Web_App_Master.Account
{
    public partial class AssetController : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Login");
            }
        }
        public static bool IsNumber(string s)
        {
            return s.All(char.IsDigit);
        }
        private static string ParseScanInput(string num)
        {
            //get setting for asset length padding

            int assetcount = Global.Library.Settings.AssetNumberLength;
                      

            var ret = num;
            if (ret.Length < assetcount)
            {
                do
                {
                    ret = "0" + ret;
                } while (ret.Length != assetcount);
            }




            return ret;
        }
        public static string ParseBarCode(string input)
        {

            int assetcount = Global.Library.Settings.AssetNumberLength;


            var ret = input;
            if (ret.Length < assetcount)
            {
                do
                {
                    ret = "0" + ret;
                } while (ret.Length != assetcount);
            }




            return ret;
        }

        [WebMethod]
        public static bool UpdateAsset(Asset asset)
        {
            try
            {
                SQL_Request req = new SQL_Request().OpenConnection();
                    req.UpdateAsset(asset);
                if (req.Success==true)
                {
                    return true;
                }
                else { return false; }

            }
            catch
            {
                return false;
            }

        }

        [WebMethod]
        public static bool AssetOnHold(string num, string b)
        {
           num = ParseBarCode(num);
            foreach(var asset in GetAllAssets())
            {
                if (asset.AssetNumber==num)
                {                    
                    try
                    {
                        asset.OnHold = Convert.ToBoolean(b);
                        SQL_Request req = new SQL_Request().OpenConnection();
                        req.UpdateAsset(asset);
                        if (req.Success == true)
                        {
                            return true;
                        }
                        else { return false; }

                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;

        }

        [WebMethod]
        public static bool AssetIsDamaged(string num, string b)
        {
            num = ParseBarCode(num);
            foreach (var asset in GetAllAssets())
            {
                if (asset.AssetNumber == num)
                {
                   
                    try
                    {
                        asset.IsDamaged = Convert.ToBoolean(b);
                        SQL_Request req = new SQL_Request().OpenConnection();
                        req.UpdateAsset(asset);
                        if (req.Success == true)
                        {
                            return true;
                        }
                        else { return false; }

                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;

        }

        [WebMethod]
        public static bool AssetIsCalibrated(string num, string b)
        {
            num = ParseBarCode(num);
            foreach (var asset in GetAllAssets())
            {
                if (asset.AssetNumber == num)
                {
                    
                    try
                    {
                        asset.IsCalibrated = Convert.ToBoolean(b);
                        SQL_Request req = new SQL_Request().OpenConnection();
                        req.UpdateAsset(asset);
                        if (req.Success == true)
                        {
                            return true;
                        }
                        else { return false; }

                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return false;

        }

        [WebMethod]
        public static Settings GetSettings(string AppName="AWP_STARRAG_US")
        {
           
            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {               
                return Global.Library.Settings;
            }
            //barcode was scanned or manually inputted
        
            SQL_Request req = new SQL_Request().OpenConnection();
            req.SettingsGet(AppName);
            if (req.Tag is SettingsDBData)
            {
                try
                {
                    var dbdata = req.Tag as SettingsDBData;
                    var settings = new Settings().DeserializeFromXmlString<Settings>(dbdata.XmlData);
                    return settings;
                }
                catch { return new Settings(); }
            }
            else
                return null;
        }

        [WebMethod]
        public static bool PushSettings( Settings settings, string AppName = "AWP_STARRAG_US")
        {
            string xmldata = settings.Serialize();
            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {
                return false;
            }
            SQL_Request req = new SQL_Request().OpenConnection();
            //barcode was scanned or manually inputted
            var sql_settings = GetSettings();
            if (sql_settings==null)
            {
                var result = req.SettingsAdd(AppName,xmldata);
                return true;
            }

            
            try
            {
                var result = req.SettingsUpdate(AppName,xmldata);
            }
            catch
            {
                return false;
            }
            finally { req.CloseConnection();  }
            return true;
        }

        [WebMethod]
        public static bool PushSetting(string xml, string guid)
        {
            string xmldata = xml;
            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {
                return false;
            }
            SQL_Request req = new SQL_Request().OpenConnection();
            //barcode was scanned or manually inputted
            var sql_settings = GetSetting(guid);
            if (sql_settings == null)
            {
                var result = req.SettingsAdd(guid, xmldata);
                return true;
            }


            try
            {
                var result = req.SettingsUpdate(guid, xmldata);
            }
            catch
            {
                return false;
            }
            finally { req.CloseConnection(); }
            return true;
        }

        [WebMethod]
        public static bool PushSetting(SettingsDBData data)
        {
            
            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {
                return false;
            }
            SQL_Request req = new SQL_Request().OpenConnection();
            //barcode was scanned or manually inputted
            var sql_settings = GetSetting(data.Appname);
            if (sql_settings == null)
            {
                var result = req.SettingsAdd(data.Appname, data.XmlData, data.XmlData2, data.XmlData3, data.XmlData4, data.XmlData5);
                return true;
            }


            try
            {
                var result = req.SettingsUpdate(data.Appname, data.XmlData, data.XmlData2, data.XmlData3, data.XmlData4, data.XmlData5);
            }
            catch
            {
                return false;
            }
            finally { req.CloseConnection(); }
            return true;
        }

        [WebMethod]
        public static SettingsDBData GetSetting(string guid)
        {

            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {
                return null;
            }
            //barcode was scanned or manually inputted

            SQL_Request req = new SQL_Request().OpenConnection();
            req.SettingsGet(guid);
            if (req.Tag is SettingsDBData)
            {
                try
                {
                    var dbdata = req.Tag as SettingsDBData;
                   
                    return dbdata;
                }
                catch { return null; }
            }
            else
                return null;
        }

        [WebMethod]
        public static bool SetTestMode(string ischecked)
        {
            try
            {
                if (Convert.ToBoolean(ischecked))
                {
                    Global.Library.Settings.TESTMODE = true;
                    Save.Globals();
                }
                else
                {
                    Global.Library.Settings.TESTMODE = false;
                    Save.Globals();
                }
                return true;
            }
            catch { return false; }
        }

        [WebMethod]
        public static void BindCheckInOutBoxes()
        {
            try
            {
                Page page = (Page)HttpContext.Current.Handler;
                var notifyView = page.Master.FindControl("notifyView") as LoginView;
                var notifyView2 = page.FindControl("notifyView") as LoginView;
                var _NoticeRepeater = notifyView.FindControl("NoticeRepeater") as Repeater;
                var _NoticeUpdate = notifyView.FindControl("NoticeUpdatePanel") as UpdatePanel;
                if (_NoticeUpdate != null)
                {
                    var checkout = HttpContext.Current.Session["CheckOut"] as List<Asset>;
                    var checkin = HttpContext.Current.Session["CheckIn"] as List<Asset>;
                    if (checkout == null) checkout = new List<Asset>();
                    if (checkin == null) checkin = new List<Asset>();
                    _NoticeRepeater.DataSource = checkout.Concat(checkin);
                    _NoticeRepeater.DataBind();
                    _NoticeUpdate.Update();
                }
            }
            catch { }
        }

        [WebMethod]
        public static Asset AddCheckoutItem(string num)
        {
            //MAYBE USE COOKIES HERE instead of session data
           num = ParseBarCode(num);
            var checkout = HttpContext.Current.Session["CheckOut"] as List<Asset>;
            

            var asset = Global.Library.Assets.FindAssetByNumber(num);
            if (asset.IsOut == true) return null;
            if (checkout == null)
                checkout = new List<Asset>();
            foreach(var a in checkout)
            {
                //catch duplicates
                if (a.AssetNumber==asset.AssetNumber)
                { return null; }
            }
            checkout.Add(asset);
            HttpContext.Current.Session["CheckOut"] = checkout;
            return asset;
        }

        [WebMethod]
        public static Asset AddCheckinItem(string num)
        {

           num = ParseBarCode(num);
            var checkin = HttpContext.Current.Session["CheckIn"] as List<Asset>;
            var asset = Global.Library.Assets.FindAssetByNumber(num);
            if (asset.IsOut == false) return null;
            if (checkin == null)
                checkin = new List<Asset>();
            foreach (var a in checkin)
            {
                //catch duplicates
                if (a.AssetNumber == asset.AssetNumber)
                { return null; }
            }
            checkin.Add(asset);
            HttpContext.Current.Session["CheckIn"] = checkin;
            return asset;
        }

        [WebMethod]
        public static Asset GetAsset(string num)
        {
            num = ParseBarCode(num);
            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {
                var a = from ass in Global.Library.Assets where ass.AssetNumber == num select ass;
                var al = a.ToList();
                var asset = al.FirstOrDefault();
                asset.Images = asset.Images.Replace(",,,", "<@#$>").Replace(",", "").Replace("<@#$>", ",").Replace("Images", "").Replace("\\", "");
                HttpContext.Current.Session["CurrentAsset"] = asset;
                return asset;
            }
            //barcode was scanned or manually inputted
            //num = ParseScanInput(num);
            SQL_Request req = new SQL_Request().OpenConnection();
            req.GetAsset(num);
            if (req.Tag is Asset)
            {
                (req.Tag as Asset).Images = (req.Tag as Asset).Images.Replace("Images", "").Replace("\\", "");
                HttpContext.Current.Session["CurrentAsset"] = req.Tag as Asset;
                return req.Tag as Asset;
            }
            else
                return new Asset();
        }

        [WebMethod]
        public static Asset GetHistory(string num, string date)
        {
            try
            {
                num = ParseBarCode(num);
                var connected = Extensions.CheckForInternetConnection();
                if (!connected)
                {
                    var a = from ass in Global.Library.Assets where ass.AssetNumber == num select ass;
                    var al = a.ToList();
                    var asset = al.FirstOrDefault();
                    asset.Images = asset.Images.Replace(",,,", "<@#$>").Replace(",", "").Replace("<@#$>", ",").Replace("Images", "").Replace("\\", "");
                    HttpContext.Current.Session["CurrentAsset"] = asset;
                    var ret = GetAssetByDateShipped(asset, date);
                    return ret;
                }
                //barcode was scanned or manually inputted
                num = ParseScanInput(num);
                SQL_Request req = new SQL_Request().OpenConnection();
                req.GetAsset(num);
                if (req.Tag is Asset)
                {
                    (req.Tag as Asset).Images = (req.Tag as Asset).Images.Replace(",,,", "<@#$>").Replace(",", "").Replace("<@#$>", ",").Replace("Images", "").Replace("\\", "");
                    HttpContext.Current.Session["CurrentAsset"] = req.Tag as Asset;
                    var ret = GetAssetByDateShipped((req.Tag as Asset), date);
                    return ret;                    
                }
                else
                    return new Asset();
            }
            catch { return new Asset(); }
        }
        private static Asset GetAssetByDateShipped(Asset a, string longdatestring)
        {
            try
            {
                foreach (var asset in a.History.History)
                {
                    if (asset.DateShipped.Ticks.ToString()==longdatestring)
                    {
                        return asset;
                    }
                }
            }
            catch { return new Asset(); }
            return new Asset();
        }
        [WebMethod]
        public static List<Asset> GetAllAssets()
        {
            //check internet for online offlne access

            var connected = Extensions.CheckForInternetConnection();
            if (!connected)
            {

                //load local
                var file =HttpContext.Current.Server.MapPath("/App_Data/library.xml");
                Extensions.ImportXmlLibraryFile(file);
                return Global.Library.Assets;
            }
            SQL_Request req = new SQL_Request().OpenConnection();

            //request all assets
            req.GetAllAssets(true);
            if (req.Tag != null)
            {
                Global.Library.Assets = req.Tag as List<Asset>;

            }
            else
            {
                var file = HttpContext.Current.Server.MapPath("/App_Data/library.xml");
                Extensions.ImportXmlLibraryFile(file);
                return Global.Library.Assets;
            }

            return Global.Library.Assets;
        }

        [WebMethod]
        public static List<Asset> AssetSearch(string SearchTerm, string SearchFilter = "Asset")
        {
            try
            {
                List<Asset> Results = new List<Asset>();
                if (SearchFilter == "Asset")
                {

                    foreach (var asset in GetAllAssets())
                    {

                        try
                        {
                            if (asset.AssetNumber != null)
                            {
                                if (asset.AssetNumber.ToUpper().Contains(SearchTerm.ToUpper()))
                                {

                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (asset.AssetName != null)
                            {
                                if (asset.AssetName.ToUpper().Contains(SearchTerm.ToUpper()))
                                {
                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (asset.Description != null)
                            {
                                if (asset.Description.ToUpper().Contains(SearchTerm.ToUpper()))
                                {
                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (asset.ShipTo != null)
                            {
                                if (asset.ShipTo.ToUpper().Contains(SearchTerm.ToUpper()))
                                {
                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (asset.ServiceEngineer != null)
                            {
                                if (asset.ServiceEngineer.ToUpper().Contains(SearchTerm.ToUpper()))
                                {
                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (asset.PersonShipping != null)
                            {
                                if (asset.PersonShipping.ToUpper().Contains(SearchTerm.ToUpper()))
                                {
                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (asset.OrderNumber != "-1")
                            {
                                if (asset.OrderNumber.ToString().ToUpper().Contains(SearchTerm.ToUpper()))
                                {
                                    if (!Results.Contains(asset))
                                        Results.Add((Asset)asset.Clone());
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    return Results;
                }
                if (SearchFilter == "History")
                {

                    foreach (var aaa in GetAllAssets())
                    {
                        try
                        {
                            foreach (var asset in aaa.History.History)
                            {
                                try
                                {
                                    if (asset.AssetNumber != null)
                                    {
                                        if (asset.AssetNumber.ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());

                                        }
                                    }
                                }
                                catch
                                {

                                }
                                try
                                {
                                    if (asset.AssetName != null)
                                    {
                                        if (asset.AssetName.ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                try
                                {
                                    if (asset.Description != null)
                                    {
                                        if (asset.Description.ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                try
                                {
                                    if (asset.ShipTo != null)
                                    {
                                        if (asset.ShipTo.ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                try
                                {
                                    if (asset.ServiceEngineer != null)
                                    {
                                        if (asset.ServiceEngineer.ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                try
                                {
                                    if (asset.PersonShipping != null)
                                    {
                                        if (asset.PersonShipping.ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());
                                        }
                                    }
                                }
                                catch
                                {

                                }
                                try
                                {
                                    if (asset.OrderNumber != "-1")
                                    {
                                        if (asset.OrderNumber.ToString().ToUpper().Contains(SearchTerm.ToUpper()))
                                        {
                                            if (!Results.Contains(asset))
                                                Results.Add((Asset)asset.Clone());
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch { }
                    }
                    return Results;
                }

                if (SearchFilter == "Date")
                {
                    return Results;
                }
            }
            catch { }
            return new List<Asset>();
        }

        [WebMethod]
        public static bool ExportLibrary()
        {
            try {

                var assetslibrary = Global.Library;
                var assets = assetslibrary.Assets;
                var length = assets.Count;
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                StringBuilder builder = new StringBuilder();
                builder.Capacity = 5000;
               
                using (XmlWriter writer = XmlWriter.Create(builder, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("AssetList");
                    foreach (var s in assets)
                    {
                        
                        try
                        {
                            AddAssetToXml(writer, s);
                        }
                        catch { }
                        
                    }
                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
                using (var sr = File.OpenWrite(assetslibrary.Name))
                using (StreamWriter s = new StreamWriter(sr))
                {
                    builder.ToString();
                    s.WriteLine(builder.ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        

        private static void AddAssetToXml(XmlWriter writer, Asset s)
        {
            try
            {
                writer.WriteStartElement("Asset");
                try
                {
                    writer.WriteAttributeString("Name", s.AssetName);
                }
                catch { }

                writer.WriteStartElement("AssetNumber");
                try
                {
                    writer.WriteString(s.AssetNumber);
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("Description");

                try
                {
                    if (s.Description.Contains("\n"))
                    {
                        s.Description = s.Description.Replace("\n", " ");
                    }
                    writer.WriteString(s.Description);
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("BarcodeImage");
                try
                {
                    writer.WriteString(s.BarcodeImage);
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("Images");
                try
                {

                    writer.WriteStartElement("Image");
                    writer.WriteString(s.Images);
                    writer.WriteEndElement();

                }
                catch (Exception ex) { }
                writer.WriteEndElement();

                writer.WriteStartElement("DateRecieved");
                try
                {
                    writer.WriteString(s.DateRecieved.ToString());

                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("DateShipped");
                try
                {
                    writer.WriteString(s.DateShipped.ToString());
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("IsOut");
                try
                {
                    writer.WriteString(s.IsOut.ToString());
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("OrderNumber");
                try
                {
                    writer.WriteString(s.OrderNumber.ToString());
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("PersonShipping");
                try
                {
                    writer.WriteString(s.PersonShipping);
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("ServiceEngineer");
                try
                {
                    writer.WriteString(s.ServiceEngineer);
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("ShipTo");
                try
                {
                    writer.WriteString(s.ShipTo);
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("Weight");
                try
                {
                    writer.WriteString(s.weight.ToString());
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("OnHold");
                try
                {
                    writer.WriteString(s.OnHold.ToString());
                }
                catch { }
                writer.WriteEndElement();

                writer.WriteStartElement("IsDamaged");
                try
                {
                    writer.WriteString(s.IsDamaged.ToString());
                }
                catch { }
                writer.WriteEndElement();


                writer.WriteStartElement("ShippingInformation");


                try
                {
                    writer.WriteStartElement("PackingSlip");
                    writer.WriteString(s.PackingSlip);
                    writer.WriteEndElement();
                }
                catch { }
                try
                {
                    writer.WriteStartElement("UPSlabel");
                    writer.WriteString(s.UpsLabel);
                    writer.WriteEndElement();
                }
                catch { }
                try
                {
                    writer.WriteStartElement("ReturnReport");
                    writer.WriteString(s.ReturnReport);
                    writer.WriteEndElement();
                }
                catch { }


                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            catch (Exception ex)
            {

            }
        }



    }


}