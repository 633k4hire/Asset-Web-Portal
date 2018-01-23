using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_App_Master.Account
{
    public partial class Checkout : System.Web.UI.Page
    {
        //Master Event Wire Up
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.SiteMaster().OnPanelUpdate += Checkout_OnPanelUpdate;
        }
        private void Checkout_OnPanelUpdate(object sender, UpdateRequestEvent e)
        {
            FinalCheckoutRepeater.DataBind();
            OutCartUpdatePanel.Update();
        }


        public string Customer = null;
        public string Shipper = null;
        public string Engineer = null;
        public string Ordernumber = null;
        public List<Asset> CheckoutItems = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PreInit += Page_PreInit;
            Button btn = (Button)this.Master.FindControl("UpdateAllCarts");
            AsyncPostBackTrigger Trigger1 = new AsyncPostBackTrigger();
            Trigger1.ControlID = btn.ID;
            Trigger1.EventName = "Click";
            if (OutCartUpdatePanel.Triggers.Count==1)
                OutCartUpdatePanel.Triggers.Add(Trigger1);

            BindCheckout();
            if(!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("/Account/Login");
                }


                Customer = Session["Customer"] as string;
                Shipper = Session["Shipper"] as string;
                Engineer = Session["Engineer"] as string;
                Ordernumber = Session["Ordernumber"] as string;
                checkout_OrderNumber.Value = Ordernumber;
                checkout_ServiceEngineer.Text = Engineer;


                try
                {
                    string usershipper = Shipper;
                    var user = Page.User.Identity.Name;
                    var split = user.Split('@');
                    user = split[0];
                    split = user.Split('.');
                    foreach (var s in split)
                    {
                        foreach (var shipper in Global.Library.Settings.ShippingPersons)
                        {
                            if (shipper.Name.ToLower().Contains(s.ToLower()) || shipper.Email.ToLower().Contains(s.ToLower()))
                            {
                                Shipper = shipper.Name;
                            }
                        }
                    }
                }
                catch { }
                checkout_ShippingPerson.Text = Shipper;

                checkout_ShipTo.Text = Customer;
                checkout_ShipTo.Focus();
            }
        }
        public void BindCheckout()
        {
            var checkout = HttpContext.Current.Session["CheckOut"] as List<Asset>;
            if (checkout == null) checkout = new List<Asset>();
            FinalCheckoutRepeater.DataSource = checkout;
            FinalCheckoutRepeater.DataBind();

            checkout_ServiceEngineer.DataSource = GetEngineerNames();
            checkout_ServiceEngineer.DataBind();

            checkout_ShippingPerson.DataSource = GetShippingNames();
            checkout_ShippingPerson.DataBind();

            checkout_ShipTo.DataSource = GetShipToNames();
            checkout_ShipTo.DataBind();
        }
        public List<string> GetShipToNames()
        {
            var names = (from ship in Global.Library.Settings.Customers orderby ship.CompanyName select ship.CompanyName).ToList();
            return names;
        }
        public List<string> GetEngineerNames()
        {
            var names = (from ship in Global.Library.Settings.ServiceEngineers orderby ship.Name select ship.Name).ToList();
            return names;
        }
        public List<string> GetShippingNames()
        {
            var names = (from ship in Global.Library.Settings.ShippingPersons orderby ship.Name select ship.Name).ToList();
            return names;
        }

        protected void ContinueToCheckOutBtn_Click(object sender, EventArgs e)
        {

            Session["Customer"] =( checkout_ShipTo.Text);
           Session["Shipper"] = (checkout_ShippingPerson.Text);
             Session["Engineer"] =(checkout_ServiceEngineer.Text);
           Session["Ordernumber"] = (checkout_OrderNumber.Value);
            if ((Session["CheckOut"]) == null) return;
            if ((Session["CheckOut"] as List<Asset>).Count == 0) return;
            Context.Response.Redirect("/Account/Checkout.aspx");
        }

        protected void FinalCheckoutRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower()=="delete")
            {
                Asset rem = null;
                foreach(var ass in Session["CheckOut"] as List<Asset>)
                {
                    if (ass.AssetNumber== e.CommandArgument as string)
                    {
                        rem = ass;
                    }
                }
                try
                {
                    (Session["CheckOut"] as List<Asset>).Remove(rem);
                }
                catch { }
                this.UpdateAll();
            }
        }

    }
   

}