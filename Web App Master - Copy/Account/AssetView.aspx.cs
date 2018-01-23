using Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Web_App_Master.Account
{
    public partial class AssetView : System.Web.UI.Page
    {
        public string IsSelected(object num)
        {

            if ((bool)num == true)
                return "element-selected";
            else
                return "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Login");
            }
            AssetViewRepeater.HeaderTemplate = Page.LoadTemplate("/Account/Templates/av_header_template.ascx");
            AssetViewRepeater.ItemTemplate = Page.LoadTemplate("/Account/Templates/av_sm_tile_template.ascx");
            AssetViewRepeater.FooterTemplate = Page.LoadTemplate("/Account/Templates/av_footer_template.ascx");
            AssetViewRepeater.DataSource = GetAssets();
            AssetViewRepeater.DataBind();
           // PlaceHolder lbl_UserName = this.Master.FindControl("RibbonBar") as PlaceHolder;
            //lbl_UserName.Visible = true;
        }
        private List<Asset> GetAssets()
        {
            //check internet for online offlne access
            SQL_Request req = new SQL_Request().OpenConnection();
            
            //request all assets
            req.GetAllData(true);
            if (req.Tag != null)
            {
                Global._Library.Assets = req.Tag as List<Asset>;

            }
            else
            {
                try
                {
                    var name = Server.MapPath("/library.xml");

                    var doc = new XmlDocument();

                    doc.LoadXml(File.ReadAllText(name));
                    XmlNodeList elemList = doc.GetElementsByTagName("Asset");
                    foreach (XmlElement asset in elemList)
                    {
                        Asset a = new Asset();
                        try
                        {

                            a.ItemName = asset.GetAttribute("Name").Sanitize();
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
                            a.OrderNumber = Convert.ToInt32(asset.SelectSingleNode("OrderNumber").InnerText.Sanitize());
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
                            XmlNodeList imglist = doc.GetElementsByTagName("Image");

                            foreach (XmlElement el in imglist)
                            {
                                a.Images+=""+(el.SelectSingleNode("").InnerText.Sanitize().SanitizeHTML())+",";
                            }
                        }
                        catch { }

                        Global._Library.Assets.Add(a);

                    }
                }
                catch { }
            }

            return Global._Library.Assets;
        }
        [System.Web.Services.WebMethod]
        public static string search()
        {
            return "asset stuff";
        }
        [System.Web.Services.WebMethod]
        public static bool parseSearch(string num)
        {

            return true;
        }

        [System.Web.Services.WebMethod]
        public static Asset getAssetInfo(string num)
        {
           
            Asset a = new Asset();
            a.ItemName = "Test Item";
            a.AssetNumber = "0077";
            a.Description = "Test Description";
            a.BarcodeImage = "../Images/0101.png";
            return a;
        }

       

        protected void AssetViewRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
              //  ((HtmlGenericControl)(e.Item.FindControl("myDiv"))).
                //   Attributes["class"] += "";
            }
        }

        protected void ChangeView_Click(object sender, EventArgs e)
        {
            switch(avSelectedView.Text)
            {
                case "list":
                    AssetViewRepeater.HeaderTemplate = Page.LoadTemplate("/Account/Templates/emptytemplate.ascx");
                    AssetViewRepeater.ItemTemplate = Page.LoadTemplate("/Account/Templates/av_list_template.ascx");
                    AssetViewRepeater.FooterTemplate = Page.LoadTemplate("/Account/Templates/emptytemplate.ascx");
                    AssetViewRepeater.DataSource = GetAssets();
                    AssetViewRepeater.DataBind();
                    break;
                case "detail":

                    break;
                case "smtile":

                    break;
                case "mdtile":

                    break;
                case "lgtile":

                    break;
            }
        }
    }
}