using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

using iTextSharp.text.pdf;

namespace Web_App_Master
{
    public class BarcodeHandler : IHttpHandler
    {
        //some stuff snipped
        public bool IsReusable
        {
            get { return false; }
        }
        public void SomeCheckImageHandler() { }

        public void ProcessRequest(HttpContext context)
        {
            string prodCode = context.Request.QueryString.Get("code");
            var w = context.Request.QueryString.Get("w");
            var h = context.Request.QueryString.Get("h");
            int width=320;
            int height=100;
            if (w!=null)
            {
                try { width = Convert.ToInt32(w); } catch { width = 320; }
            }
            if (h != null)
            {
                try { height = Convert.ToInt32(h); } catch { height = 100; }
            }

            var filename = context.Request.Url.Segments.Last().Replace(".barcode","").Replace(".Barcode", "").Replace(".BARCODE", "");
            context.Response.ContentType = "image/png";
            if (prodCode.Length > 0)
            {
                if (prodCode.Length > Global.Library.Settings.AssetNumberLength) prodCode = prodCode.Substring(0, Global.Library.Settings.AssetNumberLength);
                Barcode128 code128 = new Barcode128();
                code128.CodeType = Barcode.CODE128;
                code128.ChecksumText = true;
                code128.GenerateChecksum = true;
                code128.StartStopText = true;
                code128.Code = prodCode;
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                Bitmap resized = new Bitmap(bm, new Size(width,height));
             
                resized.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
            }

        }
    }

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
    public class PdfHandler : IHttpHandler
    {
        //some stuff snipped
        public bool IsReusable
        {
            get { return false; }
        }
        public void SomeCheckImageHandler() { }

        public void ProcessRequest(HttpContext context)
        {  
            var path = context.Request["path"];

            Byte[] buffer = File.ReadAllBytes(path);

            if (buffer != null)
            {
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("content-length", buffer.Length.ToString());
                context.Response.BinaryWrite(buffer);
            }

        }
    }

}