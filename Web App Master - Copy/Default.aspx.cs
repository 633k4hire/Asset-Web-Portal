using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web_App_Master
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void tester_Click(object sender, EventArgs e)
        {
            try
            {
                var set = Global._Settings.ChildNodes[1].SelectSingleNode("AssetNumberLength");
                int assetcount = Convert.ToInt32(set.InnerText);
            }
            catch { }
        }


        protected void testtttt_Click(object sender, EventArgs e)
        {
            var script = "<script type='text/javascript'> " +
                 "document.getElementById('" + "buttonid here"+ "').click();" +
                 "</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LinkLog", script, false);
        }
    }
}