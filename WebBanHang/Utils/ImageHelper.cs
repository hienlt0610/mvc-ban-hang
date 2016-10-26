using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Text;

namespace WebBanHang.Utils
{
    public class ImageHelper
    {
        public static String ImageUrl(String ImagePath)
        {
            if (String.IsNullOrEmpty(ImagePath)) return DefaultImage();
            String imageUrl = ImagePath;
            if (!isImageExist(imageUrl))
                return DefaultImage();
            return ResolveServerUrl(VirtualPathUtility.ToAbsolute(imageUrl),false);
        }

        public static bool isImageExist(String path){
            var absolutePath = HttpContext.Current.Server.MapPath(path);
            return File.Exists(absolutePath);
        }

        public static String DefaultImage()
        {
            return ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/images/no_image.jpg"),false);
        }

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        public static String BlankImage()
        {
            return "data:image/gif;base64,R0lGODlhAQABAAAAACwAAAAAAQABAAA=";
        }
    }
}