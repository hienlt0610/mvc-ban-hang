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
        public static String ImageUrl(String imageName)
        {
            String imageUrl = "~/Resource/" + imageName;
            if (!isImageExist(imageUrl))
                return DefaultImage();
            return imageUrl;
        }

        public static String ThumbUrl(String imageName, int width, int height)
        {
            var ext = Path.GetExtension(imageName);
            var name = Path.GetFileNameWithoutExtension(imageName);
            StringBuilder builder = new StringBuilder("~/Resource/thumbnails/");
            builder.Append(name)
                .Append("_")
                .Append(width)
                .Append("_")
                .Append(height)
                .Append(ext);
            if(!isImageExist(builder.ToString()))
                return DefaultImage();
            return builder.ToString();
        }

        public static bool isImageExist(String path){
            var absolutePath = HttpContext.Current.Server.MapPath(path);
            return File.Exists(absolutePath);
        }

        public static String DefaultImage()
        {
            return "~/Content/images/no_image.jpg";
        }
    }
}