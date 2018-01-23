using Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web_App_Master.Account;
using Web_App_Master.Models;

namespace Web_App_Master
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void testbtn_Click(object sender, EventArgs e)
        {
            var a = AssetController.GetAsset("0007");
            if(a.IsOut)
            {
                //Asset Is Out
                a.IsOut = false;
                AssetController.UpdateAsset(a);
                a = AssetController.GetAsset("0007");
            }
            else
            {
                //Asset Is Not Out
                a.IsOut = true;
                AssetController.UpdateAsset(a);
                a = AssetController.GetAsset("0007");
            }
        }
    }
}