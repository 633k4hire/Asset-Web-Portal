using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Helpers;
using System.Xml;
using System.Linq;

namespace Web_App_Master
{
    public partial class SiteMaster : MasterPage
    {
        public List<Asset> LocalAssets { get; set; }
        public bool Loggedin { get; set; }
        public XmlDocument LocalXmlLibrary { get; set; }
        public Asset _Asset { get; set; }
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;
        public Repeater _NoticeRepeater;
        public UpdatePanel _NoticeUpdate;
        public void BindCheckout()
        {
            var checkout = HttpContext.Current.Session["CheckOut"] as List<Asset>;
            if (checkout == null) checkout = new List<Asset>();
            CheckoutRepeater.DataSource = checkout;
            CheckoutRepeater.DataBind();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            
            try
            {
                //BindCheckInOutBoxes();
            }
            catch { }
            _Asset = new Asset();
            Loggedin = false;
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindCheckout();
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
        [System.Web.Services.WebMethod]
        public static string search()
        {
            return "worked master";
        }


        public System.Collections.IEnumerable NoticeRepeater_GetData()
        {
            return null;
        }

        protected void NotifyRefreshBtn_Click(object sender, EventArgs e)
        {
        }

        protected void button33_Click(object sender, EventArgs e)
        {
            BindCheckout();
        }
    }

}