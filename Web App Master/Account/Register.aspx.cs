using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Web_App_Master.Models;
using System.IO;

namespace Web_App_Master.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            if (!Global.Library.Settings.TESTMODE)
            {
                var pass = Password.Text;
                var name = Email.Text.Split('@')[0].Replace(".", "");
                var dest = Server.MapPath("/App_Data/" + name + ".xml");
                File.WriteAllText(dest, "<user>" + user.Email + "</user><pass>" + pass + "</pass>" + Environment.NewLine);
            }
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //create body and subject

                //send email
                EmailHelper.SendEmailNoticeAsync(user.Email, "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.", "Confirm your account");
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");
                if (user.EmailConfirmed)
                {
                  signInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = "An email has been sent to your account. Please view the email and confirm your account to complete the registration process.";
                }
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}