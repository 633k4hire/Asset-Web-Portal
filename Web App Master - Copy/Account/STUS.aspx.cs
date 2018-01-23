using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Web_App_Master.Account
{
    public partial class STUS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Login");
            }
        }

        protected void UploadLibrary_Click(object sender, EventArgs e)
        {
            var name = Server.MapPath("/library.xml");
            LibraryUploader.PostedFile.SaveAs(name);
            var doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(LibraryUploader.PostedFile.InputStream))
            {
                doc.LoadXml(reader.ReadToEnd());
            }
            //Global._XmlLibrary = doc;
            Master.LocalXmlLibrary = doc;
            Master.LocalAssets = new List<Asset>();
            
            XmlNodeList elemList = doc.GetElementsByTagName("Asset");
            foreach (XmlElement asset in elemList)
            {
                Asset a = new Asset();
                try { 

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
                    a.DateRecieved = DateTime.Parse( asset.SelectSingleNode("DateRecieved").InnerText.Sanitize());
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
                try { 
                a.ServiceEngineer = asset.SelectSingleNode("ServiceEngineer").InnerText.Sanitize();
                }
                catch { }
                try
                {
                    a.ShipTo = asset.SelectSingleNode("ShipTo").InnerText.Sanitize();
                }
                catch { }
                try { 
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
                        a.Images+=""+el.InnerText+",";
                    }
                }
                catch { }

                Global._Library.Assets.Add(a);

            }
            try
            {
                var T = Global._Library.Assets;
                Notification.Text = "Upload Complete";                
                MessageHolder.Visible = true;


            }
            catch { }
            //string someScript = "<script type='text / javascript'> function UploadCompleted() {  $.Notify({caption: 'Upload Complete',content: 'Asset Library Ready', icon: '<i class='glyphicon glyphicon-wrench'></i>});}; </script>";
            var script = @"
            <script type='text/javascript'> 
            function Completed() {
                        $.Notify({
                            caption: 'Upload Complete',
                            content: 'Library File'
                        });
            };
            Completed();
            </script>
            ";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", script);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "Completed();", true);
        }

        protected void SendSQL_Click(object sender, EventArgs e)
        {
            SQL_Request req = new SQL_Request().OpenConnection();

            //request all assets
            req.GetAllData(false);


            //merge all assets

            //post merged assets as new master DB
            if (req.Tag != null)
            {
                var cloud = req.Tag as List<Asset>;
                foreach (Asset a in Global._Library.Assets)
                {
                    try
                    {
                        var lookup = cloud.FindAssetByNumber(a.AssetNumber);
                        if (lookup == null)
                        {
                            req.AddAsset(a, false);
                            
                        }else
                        if (lookup.AssetNumber == a.AssetNumber)
                        {
                            req.UpdateData(a, false);
                            
                        }
                    }
                    catch { Notification.Text = "SQL-LINK Error"; }
                }
                Notification.Text = "SQL-LINK Complete";
            }
            req.CloseConnection();

            
        }

        protected void PullSQL_Click(object sender, EventArgs e)
        {
            try {
                SQL_Request req = new SQL_Request().OpenConnection();

                //request all assets
                req.GetAllData(true);
                if (req.Tag != null)
                {
                    Global._Library.Assets = req.Tag as List<Asset>;
                    Notification.Text = "SQL Pull Success";
                }
            }
            catch
            {
                Notification.Text = "SQL Pull Error";
            }
        }

        protected void DeleteSQL_Click(object sender, EventArgs e)
        {
            SQL_Request req = new SQL_Request().OpenConnection();
            try
            {
                

                //request all assets
                req.GetAllData(false);
                if (req.Tag != null)
                {
                    var cloud = req.Tag as List<Asset>;

                    foreach(var asset in cloud)
                    {
                        try
                        {
                            req.DeleteData(asset.AssetNumber, false);
                        }
                        catch {  Notification.Text = "SQL Erase Error";}
                    }

                    req.CloseConnection();
                    Notification.Text = "SQL Erase Success";
                }
            }
            catch
            {
                
                Notification.Text = "SQL Erase Error";
            }
            finally
            {
                req.CloseConnection();
            }
            
        }
    }
}