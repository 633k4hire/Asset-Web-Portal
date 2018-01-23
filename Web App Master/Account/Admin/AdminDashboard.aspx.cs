using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_App_Master.Models;
using Helpers;
using System.Xml;
using System.IO;

namespace Web_App_Master.Account.Admin
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        private void UpdateUsersAndRoles()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!roleManager.RoleExists("NoRole"))
            {
                roleManager.Create(new IdentityRole("NoRole"));
            }
            var nonRoledUsers = (from user in manager.Users where user.Roles.Count == 0 orderby user.Email select user).ToList();
            foreach (var user in nonRoledUsers)
            {
                manager.AddToRole(user.Id, "NoRole");
            }

            var roles = (from role in roleManager.Roles orderby role.Name select role).ToRoleBindingList();

            RolesAndUsersRepeater.DataSource = roles;
            RolesAndUsersRepeater.DataBind();

            var users = (from user in manager.Users orderby user.Email select user).ToUserBindingList();
            UserRepeater.DataSource = users;
            UserRepeater.DataBind();


        }
        private void UpdateAssetAdmin()
        {
            try
            {
                ups_pwd.Value = Global.Library.Settings.UpsAccount.P;
                ups_aln.Value = Global.Library.Settings.UpsAccount.A;
                ups_userid.Value = Global.Library.Settings.UpsAccount.I;
                ups_shippernumber.Value = Global.Library.Settings.UpsAccount.N;
            }
            catch { }
            try
            {
                shipmsgbox.Value = Global.Library.Settings.ShipperNotification;
                checkinmsgbox.Value = Global.Library.Settings.CheckInMessage;
                checkoutmsgbox.Value = Global.Library.Settings.CheckOutMessage;
                notificationmsgbox.Value = Global.Library.Settings.NotificationMessage;
            }
            catch
            {

            }
            try
            {
                if (Global.Library.Settings.Notifications_30_Day != null)
                {
                    var list = (from n in Global.Library.Settings.Notifications_30_Day orderby n.Time select n).ToList();
                    AssetNoticeBindinglist list30day = new AssetNoticeBindinglist(list);

                    Notice30DayRepeater.DataSource = list30day;
                    Notice30DayRepeater.DataBind();
                }
            }
            catch
            {
                PopNotify("Error", "Could Not Bind 30 Day Notices");
            }
            try
            {
                if (Global.Library.Settings.Notifications_15_Day != null)
                {
                    var list = (from n in Global.Library.Settings.Notifications_15_Day orderby n.Time select n).ToList();
                    AssetNoticeBindinglist list15day = new AssetNoticeBindinglist(list);

                    Notice15DayRepeater.DataSource = list15day;
                    Notice15DayRepeater.DataBind();
                }
            }
            catch
            {
               PopNotify("Error", "Could Not Bind 15 Day Notices");
            }
            try
            {
                if (Global.Library.Settings.ShippingPersons != null)
                {
                    var list = (from n in Global.Library.Settings.ShippingPersons orderby n.Name select n).ToList();

                    EmailBindinglist emailList = new EmailBindinglist(list);
                    ShippingPersonRepeater.DataSource = emailList;
                    ShippingPersonRepeater.DataBind();
                }
            }
            catch
            {
                PopNotify("Error", "Could Not Bind Shipping Personnel");
            }
            try
            {
                if (Global.Library.Settings.ServiceEngineers != null)
                {
                    var list = (from n in Global.Library.Settings.ServiceEngineers orderby n.Name select n).ToList();
                    EmailBindinglist emailList = new EmailBindinglist(list);
                    EngineerRepeater.DataSource = emailList;
                    EngineerRepeater.DataBind();
                }
            }
            catch
            {
                PopNotify("Error", "Could Not Bind Engineers");
            }
            BindCustomers();

        }
        protected void BindCustomers()
        {
            try
            {
                if (Global.Library.Settings.Customers != null)
                {
                    var list = (from n in Global.Library.Settings.Customers orderby n.CompanyName select n).ToList();
                    CustomerBindinglist custlist = new CustomerBindinglist(list);
                    CustomerRepeater.DataSource = custlist;
                    CustomerRepeater.DataBind();
                }
            }
            catch
            {
                PopNotify("Error", "Could Not Bind Customers");
            }
        }
        protected void ShowError(string error)
        {
            try
            {
                MessagePlaceHolder.Visible = true;
                ErrorMsg.Text = error;
            }
            catch { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {   if (!IsPostBack)
                {
                UpdateUsersAndRoles();
                UpdateAssetAdmin();
            }
           
        }
        
        private List<ApplicationUser> GetUsersForRole(string role)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            foreach(var user in manager.Users)
            {
                foreach(var r in user.Roles)
                {
                    if (r.RoleId==role)
                    {
                        users.Add(user);
                    }
                    if (r.UserId==user.Id)
                    {
                        users.Add(user);
                    }
                }
            }
            return users;
        }
        private bool AddUserToRole(string username, string role)
        {
            try
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var user = manager.AddToRole(username, role);
                return true;
            }
            catch 
            {

                return false;
            }
        }
        private bool RemoveUserFromRole(string userid, string roleid)
        {
            try
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
               var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                /*var irole = roleManager.FindById(role);
               IdentityUserRole remItem=null;
               foreach (var u in irole.Users)
               {
                   if (u.UserId==userid && u.RoleId==role)
                   {
                       remItem = u;
                   }
               }
               var aa = irole.Users.Remove(remItem);
               */
                string rolename = "";
                foreach (var r in roleStore.Roles.ToList())
                {
                    if (r.Id==roleid)
                    { rolename = r.Name; }
                }
                var user = manager.RemoveFromRole(userid, rolename);
                return user.Succeeded;
            }
            catch
            {

                return false;
            }
        }
        private bool RemoveUser(string userid)
        {
            try
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                if (userid == null)
                {
                    return false;
                }

                var user = manager.FindById(userid);
                var logins = user.Logins;
                var rolesForUser = manager.GetRoles(userid);

                using (var transaction = new ApplicationDbContext().Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        manager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = manager.RemoveFromRole(user.Id, item);
                        }
                    }

                    manager.Delete(user);
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        protected void CreateRoleButton_Click(object sender, EventArgs e)
        {
            try
            {
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                //roleManager.Create(new IdentityRole(RoleName.Text));             
               
                UpdateAssetAdmin();
            }
            catch {
                ShowError("Could Not Create Role.");
               
            }
           
        }

        protected void DeleteUser_Command(object sender, CommandEventArgs e)
        {
            var a = e.CommandArgument as string;
            var b = e.CommandName as string;
            RemoveUser(b);
            UpdateUsersAndRoles();
        }
        protected bool ChangeUserRole(string userid, string role)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            try
            {
                if (!manager.IsInRole(userid, role))
                {
                    manager.AddToRole(userid, role);
                    UpdateUsersAndRoles();
                }
                return true;
            }
            catch {
                return false;
            }
        }
        protected void ChangeRole_Command(object sender, CommandEventArgs e)
        {
            var roleId = e.CommandArgument as string;
            var name = e.CommandName as string;
            try
            {
                foreach (RepeaterItem i in RolesAndUsersRepeater.Items)
                {

                    Repeater repeater = (Repeater)i.Controls[1];
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        var changebutton = item.FindControl("ChangeRole") as Button;
                        var temprole = changebutton.CommandName;
                        if (temprole == name)
                        {
                            var dropdown = item.FindControl("RoleDropDown") as DropDownList;
                            if (dropdown != null)
                            {
                                var selected = dropdown.SelectedValue;
                                
                                var result = ChangeUserRole(name, selected);
                                result = RemoveUserFromRole(name, roleId);

                                return;
                            }
                        }

                    }

                }
            }
            catch { }
        }
        protected List<string> GetRoleNames()
        {
            List<string> names = new List<string>();
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            foreach(var role in roleStore.Roles)
            {
                names.Add(role.Name);
            }
            return names;
        }

        protected void RolesAndUsersRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void DeleteFromRole_Command(object sender, CommandEventArgs e)
        {
            var a = e.CommandArgument as string;
            var b = e.CommandName as string;
            RemoveUserFromRole(b, a);
            UpdateUsersAndRoles();
        }

        protected void CopyRole_Command(object sender, CommandEventArgs e)
        {
            var roleId = e.CommandArgument as string;
            var name = e.CommandName as string;
            try
            {
                foreach (RepeaterItem i in RolesAndUsersRepeater.Items)
                {

                    Repeater repeater = (Repeater)i.Controls[1];
                    foreach (RepeaterItem item in repeater.Items)
                    {
                        var changebutton = item.FindControl("ChangeRole") as Button;
                        var temprole = changebutton.CommandName;
                        if (temprole == name)
                        {
                            var dropdown = item.FindControl("RoleDropDown") as DropDownList;
                            if (dropdown != null)
                            {
                                var selected = dropdown.SelectedValue;
                                ChangeUserRole(name, selected);
                                return;
                            }
                        }

                    }

                }
            }
            catch { }
            UpdateUsersAndRoles();
        }

        protected void UsersAndRolesBtn_Click(object sender, EventArgs e)
        {
            AdminMultiView.ActiveViewIndex = 0;
            UpdateUsersAndRoles();
        }

        protected void AssetAdminBtn_Click(object sender, EventArgs e)
        {
            AdminMultiView.ActiveViewIndex = 1;
            UpdateAssetAdmin();
        }

        protected void CustomerManager_Click(object sender, EventArgs e)
        {
            AdminMultiView.ActiveViewIndex = 3;
            BindCustomers();
        }

        protected void UploadLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                var name = Server.MapPath("/App_Data/library.xml");
                
                LibraryUploader.PostedFile.SaveAs(name);
                var doc = new XmlDocument();
                using (StreamReader reader = new StreamReader(LibraryUploader.PostedFile.InputStream))
                {
                    doc.LoadXml(reader.ReadToEnd());
                }
               

                try
                {
                    XmlNodeList elemList = doc.GetElementsByTagName("Asset");
                    foreach (XmlElement elem in elemList)
                    {
                        var a = elem.ToAsset();
                        Global.Library.Assets.Add(a);

                    }
                }
                catch
                {
                    //PopNotify("Error", "Error Adding Assets");

                }
                try
                {
                    if (Global.Library.Settings == null) Global.Library.Settings = new Settings();
                    if (Global.Library.Settings.Customers == null) Global.Library.Settings.Customers = new List<Customer>();
                    XmlNodeList custElemList = doc.GetElementsByTagName("Customer");
                    foreach (XmlElement cust in custElemList)
                    {
                        //create a customer
                        Customer newCustomer = new Customer();
                        newCustomer.AccountNumber = cust.Attributes["AccountNumber"].Value;
                        newCustomer.AccPostalCd = cust.Attributes["AccPostalCd"].Value;
                        newCustomer.Address = cust.Attributes["Address"].Value;
                        newCustomer.Address2 = cust.Attributes["Address2"].Value;
                        newCustomer.Address3 = cust.Attributes["Address3"].Value;
                        newCustomer.Attn = cust.Attributes["Attn"].Value;
                        newCustomer.City = cust.Attributes["City"].Value;
                        newCustomer.CompanyName = cust.Attributes["CompanyName"].Value;
                        newCustomer.ConsInd = cust.Attributes["ConsInd"].Value;
                        newCustomer.Country = cust.Attributes["Country"].Value;
                        newCustomer.EmailAddress = new EmailAddress() { Email = cust.Attributes["Email"].Value };
                        newCustomer.Fax = cust.Attributes["Fax"].Value;
                        newCustomer.LocID = cust.Attributes["LocID"].Value;
                        newCustomer.NickName = cust.Attributes["NickName"].Value;
                        newCustomer.OrderNumber = cust.Attributes["OrderNumber"].Value;
                        newCustomer.PackageWeight = cust.Attributes["PackageWeight"].Value;
                        newCustomer.Phone = cust.Attributes["Phone"].Value;
                        newCustomer.Postal = cust.Attributes["Postal"].Value;
                        newCustomer.ResInd = cust.Attributes["ResInd"].Value;
                        newCustomer.State = cust.Attributes["State"].Value;
                        newCustomer.USPSPOBoxIND = cust.Attributes["USPSPOBoxIND"].Value;
                        Global.Library.Settings.Customers.Add(newCustomer);
                    }
                }
                catch
                {
                    //PopNotify("Error", "Error adding customers");

                }
                //get engineers
                try
                {
                    XmlNodeList engElemList = doc.GetElementsByTagName("Engineer");
                    foreach (XmlElement eng in engElemList)
                    {
                        //create a customer
                        EmailAddress newCustomer = new EmailAddress();
                        newCustomer.Email = eng.Attributes["Email"].Value;
                        newCustomer.Name = eng.Attributes["Name"].Value;

                        Global.Library.Settings.ServiceEngineers.Add(newCustomer);
                    }
                }
                catch
                {
                    //PopNotify("Error", "Error adding Engineers");

                }
                try
                {
                    if (Global.Library.Settings.ShippingPersons == null) Global.Library.Settings.ShippingPersons = new List<EmailAddress>();
                    //get shipping personel
                    XmlNodeList shipElemList = doc.GetElementsByTagName("Shipper");
                    foreach (XmlElement eng in shipElemList)
                    {
                        //create a customer
                        EmailAddress newCustomer = new EmailAddress();
                        newCustomer.Email = eng.Attributes["Email"].Value;
                        newCustomer.Name = eng.Attributes["Name"].Value;

                        Global.Library.Settings.ShippingPersons.Add(newCustomer);
                    }
                }
                catch
                {
                    //PopNotify("Error", "Error adding Shipping Personnel");

                }

                //get settings

                try
                {
                    if (Global.Library.Settings.StaticEmails == null) Global.Library.Settings.StaticEmails = new List<EmailAddress>();
                    //get shipping personel
                    XmlNodeList staticemails = doc.GetElementsByTagName("StaticEmails");
                    foreach (XmlElement staticemail in staticemails)
                    {
                        try
                        {
                            XmlNodeList emails = doc.GetElementsByTagName("StaticEmail");
                            //create a customer
                            EmailAddress newStatic = new EmailAddress();
                            newStatic.Email = staticemail.Attributes["Email"].Value;
                            newStatic.Name = staticemail.Attributes["Name"].Value;

                            Global.Library.Settings.StaticEmails.Add(newStatic);
                        }
                        catch { }

                    }

                    string outmsg = doc.GetElementsByTagName("CheckOutMessage").Item(0).InnerText.Sanitize();
                    Global.Library.Settings.CheckOutMessage = outmsg;

                    string inmsg = doc.GetElementsByTagName("CheckInMessage").Item(0).InnerText.Sanitize();
                    Global.Library.Settings.CheckInMessage = inmsg;

                    string noticemsg = doc.GetElementsByTagName("NotificationMessage").Item(0).InnerText.Sanitize();
                    Global.Library.Settings.NotificationMessage = noticemsg;

                    string shippernotice = doc.GetElementsByTagName("ShipperNotification").Item(0).InnerText.Sanitize();
                    Global.Library.Settings.ShipperNotification = shippernotice;
                }
                catch
                {
                    //PopNotify("Error", "Error adding Shipping Personnel");

                }

                UpdateAssetAdmin();
            }
            catch
            {
                //PopNotify("Error", "Upload of Library Failed");

            }

        }
        
        protected void UploadNotices_Click(object sender, EventArgs e)
        {
            var name = Server.MapPath("/App_Data/notice"+DateTime.Now.ToShortDateString().Replace(" ","").Replace("/", "-") + ".xml");
            NoticeUploader.PostedFile.SaveAs(name);
            var doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(NoticeUploader.PostedFile.InputStream))
            {
                if (reader.EndOfStream) return;
                doc.LoadXml(reader.ReadToEnd());
            }
            //check for nulls
            if (Global.Library.Settings == null) Global.Library.Settings = new Settings();
            if (Global.Library.Settings.Notifications_30_Day == null) Global.Library.Settings.Notifications_30_Day = new List<AssetNotification>();
            if (Global.Library.Settings.Notifications_15_Day == null) Global.Library.Settings.Notifications_15_Day = new List<AssetNotification>();

            XmlNodeList elemList = doc.GetElementsByTagName("Notice");
            foreach (XmlElement elem in elemList)
            {
                AssetNotification a =elem.ToNotification();
                if (a.Is30Day && a.Emails.Count>0)
                {
                    Global.Library.Settings.Notifications_30_Day.Add(a);
                }
                if (a.Is15Day && a.Emails.Count > 0)
                {
                    Global.Library.Settings.Notifications_15_Day.Add(a);
                }
            }
            UpdateAssetAdmin();
        }

        protected void UploadHistory_Click(object sender, EventArgs e)
        {
            try
            {
                var name = Server.MapPath("/App_Data/history" + DateTime.Now.ToShortDateString().Replace(" ", "").Replace("/", "-") + ".xml");
                HistoryUploader.PostedFile.SaveAs(name);
                var doc = new XmlDocument();
                using (StreamReader reader = new StreamReader(HistoryUploader.PostedFile.InputStream))
                {
                    if (reader.EndOfStream) return;
                    doc.LoadXml(reader.ReadToEnd());
                }
                //check for nulls
                if (Global.Library.Assets == null)
                {
                    //PopNotify("Error", "No Library loaded");
                    return;
                }
                if (Global.Library.Assets.Count == 0)
                {
                    //PopNotify("Error", "No Assests In Library");
                    return;
                }

                XmlNodeList elemList = doc.GetElementsByTagName("HistoryItems");
                foreach (XmlElement elem in elemList)
                {
                    try
                    {
                        var assetnumber = elem.GetAttribute("AssetNumber").Sanitize();
                        var currentAsset = Global.Library.Assets.FindAssetByNumber(assetnumber);
                        foreach (XmlElement subelem in elem.GetElementsByTagName("Asset"))
                        {
                            try
                            {
                                Asset a = subelem.ToAsset();
                                a.IsHistoryItem = true;
                                currentAsset.History.History.Add(a);
                            }
                            catch
                            {
                                //PopNotify("Error", "Adding History Item To Library Asset");
                                return;
                            }
                        }
                    }
                    catch
                    {
                        //PopNotify("Error", "Loading HistoryItems");
                        return;
                    }

                }
                try
                {

                    //PopNotify("File Upload Complete", "History file upload complete");
                }
                catch { }
            }
            catch
            {
                //PopNotify("Error", "History file upload Error");

            }
        }

        protected void PopNotify(string caption, string content)
        {
            var script = @"
            <script type='text/javascript'> 
            function Completed() {
                        $.Notify({
                            caption:'"+caption + @"',
                            content: '" + content + @"'
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
            try
            {
                SQL_Request req = new SQL_Request().OpenConnection();

                //request all assets
                req.GetAllAssets(false);


                //merge all assets

                //post merged assets as new master DB
                if (req.Tag != null)
                {
                    var cloud = req.Tag as List<Asset>;
                    //upload assets
                    foreach (Asset a in Global.Library.Assets)
                    {
                        try
                        {
                            var lookup = cloud.FindAssetByNumber(a.AssetNumber);
                            if (lookup == null)
                            {
                                req.AddAsset(a, false);

                            }
                            else
                            if (lookup.AssetNumber == a.AssetNumber)
                            {
                                req.UpdateAsset(a, false);

                            }
                        }
                        catch { //PopNotify("Error", "Error Pushing Library To SQL");
                        }
                    }
                    //upload settings
                    //upload history
                    //upload notifications
                    //upload Calibrations
                    
                }
                req.CloseConnection();

            }
            catch
            {
                //PopNotify("Error", "Error Pushing Library To SQL");
            }
            //PopNotify("Complete", "Library Pushed To SQL");
        }

        protected void PullSQL_Click(object sender, EventArgs e)
        {
            try
            {
                SQL_Request req = new SQL_Request().OpenConnection();

                //request all assets
                req.GetAllAssets(true);
                if (req.Tag != null)
                {
                    //pull assets
                    Global.Library.Assets = req.Tag as List<Asset>;
                    //pull settings
                    //pull notifications
                    //pull calibrations
                    //pull history
                }
            }
            catch
            {
                //PopNotify("Error", "Error Pulling Library From SQL");
            }
            //PopNotify("Complete", "Library Pulled From SQL");
            UpdateAssetAdmin();
        }

        protected void DeleteSQL_Click(object sender, EventArgs e)
        {
            SQL_Request req = new SQL_Request().OpenConnection();
            try
            {


                //request all assets
                req.GetAllAssets(false);
                if (req.Tag != null)
                {
                    var cloud = req.Tag as List<Asset>;

                    foreach (var asset in cloud)
                    {
                        try
                        {
                            req.DeleteAsset(asset.AssetNumber, false);
                        }
                        catch { //PopNotify("Error", "Error Deleting SQL Library");
                        }
                    }

                    req.CloseConnection();
                    Global.Library.Assets = new List<Asset>();
                }
            }
            catch
            {
                //PopNotify("Error", "Error Deleting SQL Library");
            }            
            finally
            {
                req.CloseConnection();
                //PopNotify("Complete", "SQL Library Deleted");
            }


        }

        protected void SaveUpsAcctBtn_Click(object sender, EventArgs e)
        {
            if (Global.Library.Settings.UpsAccount == null)
            { Global.Library.Settings.UpsAccount = new UPSaccount(); }
            Global.Library.Settings.UpsAccount.P = ups_pwd.Value;
            Global.Library.Settings.UpsAccount.A = ups_aln.Value;
            Global.Library.Settings.UpsAccount.I = ups_userid.Value;
            Global.Library.Settings.UpsAccount.N = ups_shippernumber.Value;
            Save.Globals();
            UpdateAssetAdmin();
        }

        protected void SaveAllChangesBtn_Click(object sender, EventArgs e)
        {
            Save.Library();
            Save.Globals();
            UpdateUsersAndRoles();
            UpdateAssetAdmin();
        }

        protected void DeleteNotice30DayBtn_Command(object sender, CommandEventArgs e)
        {
            int i = 0;
            UpdateAssetAdmin();
        }

        protected void SendNotice30DayBtn_Command(object sender, CommandEventArgs e)
        {
            UpdateAssetAdmin();
        }

        protected void DeleteNotice15DayBtn_Command(object sender, CommandEventArgs e)
        {
            UpdateAssetAdmin();
        }

        protected void SendNotice15DayBtn_Command(object sender, CommandEventArgs e)
        {
            UpdateAssetAdmin();
        }

        protected void DeleteShippingPersonBtn_Command(object sender, CommandEventArgs e)
        {
            UpdateAssetAdmin();
        }

        protected void DeleteEngineer_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeleteEngineer")
            {


            }
            UpdateAssetAdmin();
        }

        protected void CustomerRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCustomer")
            {


            }
            UpdateAssetAdmin();
        }

        protected void SendSettingsSQL_Click(object sender, EventArgs e)
        {           
            AssetController.PushSettings(Global.Library.Settings);
            //PopNotify("Success", "Settings Pushed To SQL");
            UpdateAssetAdmin();
        }

        protected void PullSettings_Click(object sender, EventArgs e)
        {
            Global.Library.Settings = AssetController.GetSettings();
            //PopNotify("Success", "Settings Pulled From SQL");
            UpdateAssetAdmin();
        }

        protected void DeleteSettingsSQL_Click(object sender, EventArgs e)
        {
            try
            {
                SQL_Request req = new SQL_Request().OpenConnection();
                req.SettingsDelete();
                Global.Library.Settings = new Settings();
            }
            catch { }
        }

        protected void AssetPeopleManager_Click(object sender, EventArgs e)
        {

            AdminMultiView.ActiveViewIndex = 2;
            UpdateAssetAdmin();
        }

        protected void NotificationManagerBtn_Click(object sender, EventArgs e)
        {
            AdminMultiView.ActiveViewIndex = 4;
            UpdateAssetAdmin();
        }

        protected void SaveCheckOutMsgBtn_Click(object sender, EventArgs e)
        {
            var a = checkoutmsgbox.Value;
            Global.Library.Settings.CheckOutMessage = a;
            Save.Globals();
        }

        protected void SaveCheckInMsgBtn_Click(object sender, EventArgs e)
        {
            Global.Library.Settings.CheckInMessage = checkinmsgbox.Value;
            Save.Globals();
        }

        protected void SaveNoticMsgBtn_Click(object sender, EventArgs e)
        {
            Global.Library.Settings.NotificationMessage = notificationmsgbox.Value;
            Save.Globals();
        }

        protected void SaveShipperMsgBtn_Click(object sender, EventArgs e)
        {
            Global.Library.Settings.ShipperNotification = shipmsgbox.Value;
            Save.Globals();
        }

    }
    
}