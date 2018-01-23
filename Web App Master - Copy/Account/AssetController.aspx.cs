using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            var set = Global._Settings.ChildNodes[1].SelectSingleNode("AssetNumberLength");
            int assetcount = Convert.ToInt32(set.InnerText);
           
            string ret = "0000";
            if (IsNumber(num))
            {
                ret = num;
                ret.Sanitize().SanitizeHTML();
                if (ret.Length<assetcount)
                {
                    do
                    {
                        ret = "0" + ret;
                    } while (ret.Length != assetcount);
                }
            }
            else
            {
                //is not a number?
            }


            return ret;
        }
        public static string ParseBarCode(string input)
        {
            var set = Global._Settings.ChildNodes[1].SelectSingleNode("AssetNumberLength");
            int assetcount = Convert.ToInt32(set.InnerText);

            string ret = "0000";
            if (IsNumber(input))
            {
                ret = input;
                ret.Sanitize().SanitizeHTML();
                if (ret.Length < assetcount)
                {
                    do
                    {
                        ret = "0" + ret;
                    } while (ret.Length != assetcount);
                }
                return ret;
            }
            return "0000";
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
            
            num = ParseBarCode(num);
            var checkout = HttpContext.Current.Session["CheckOut"] as List<Asset>;
            var asset = Global._Library.Assets.FindAssetByNumber(num);
            if (checkout == null)
                checkout = new List<Asset>();
            checkout.Add(asset);
            HttpContext.Current.Session["CheckOut"] = checkout;
            return asset;
        }
        [WebMethod]
        public static Asset AddCheckinItem(string num)
        {

            num = ParseBarCode(num);
            var checkin = HttpContext.Current.Session["CheckIn"] as List<Asset>;
            var asset = Global._Library.Assets.FindAssetByNumber(num);
            if (checkin == null)
                checkin = new List<Asset>();
            checkin.Add(asset);
            HttpContext.Current.Session["CheckIn"] = checkin;
            return asset;
        }
        [WebMethod]
        public static Asset GetAsset(string num)
        {
            //barcode was scanned or manually inputted
            num = ParseScanInput(num);
            SQL_Request req = new SQL_Request().OpenConnection();
            req.GetData(num);
            if (req.Tag is Asset)
            {
                (req.Tag as Asset).Images = (req.Tag as Asset).Images.Replace(",,,", "<@#$>").Replace(",", "").Replace("<@#$>", ",").Replace("Images","").Replace("\\","");
                HttpContext.Current.Session["CurrentAsset"] = req.Tag as Asset;
                return req.Tag as Asset;
            }
            else
                return new Asset();
        }
        [WebMethod]
        public static List<Asset> GetAllAssets()
        {
            SQL_Request req = new SQL_Request().OpenConnection();
            req.GetAllData();
            if (req.Success)
            {
                List<Asset> assets = new List<Asset>();
                foreach( DataRow row in req.Data.Tables[0].Rows)
                {
                    Asset a = new Asset();
                    a.ItemName = row.Field<string>("ItemName");
                    assets.Add(a);
                }
                return assets;
            }
            return null;
        }
    }
    

}