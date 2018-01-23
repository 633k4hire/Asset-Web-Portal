using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Web_App_Master
{
    public class ImageHandler : IHttpHandler
    {
        //some stuff snipped
        public bool IsReusable
        {
            get { return false; }
        }
        public void SomeCheckImageHandler() { }

        public void ProcessRequest(HttpContext context)
        {

            var id = context.Request["Id"];
            context.Response.ContentType = "image/png";
            var idx = context.Request["idx"];
            if (idx != null)
            {
                var apth = context.Server.MapPath(id + "/");
                if (Directory.Exists(apth))
                {
                    string[] filePaths = System.IO.Directory.GetFiles(apth);
                    List<Image> files = new List<Image>();
                    foreach (string filePath in filePaths)
                    {
                        string fileName = System.IO.Path.GetFileName(filePath);
                        var ext = System.IO.Path.GetExtension(fileName).ToUpper();
                        if (ext.Equals(".PNG"))
                        {
                            files.Add(Image.FromFile(filePath));
                        }

                    }
                    files[Convert.ToInt32(idx)].Save(context.Response.OutputStream, ImageFormat.Png);
                }
            }

        }
    }

}