using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Utils
{
    public static class UrlHelpers
    {
        public static String QueryString(this UrlHelper url,String key, String value) {
            String qString = HttpContext.Current.Request.QueryString.ToString();
            Dictionary<String, object> dict = new Dictionary<string, object>();
            HttpContext.Current.Request.QueryString.CopyTo(dict);
            dict.Remove("page");
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
            else
            {
                dict[key] = value;
            }

            return string.Join("&", dict.Select(x => x.Key + "=" + x.Value).ToArray());
        }

        public static String QueryString(this UrlHelper url)
        {
            String qString = HttpContext.Current.Request.QueryString.ToString();
            Dictionary<String, object> dict = new Dictionary<string, object>();
            HttpContext.Current.Request.QueryString.CopyTo(dict);
            return string.Join("&", dict.Select(x => x.Key + "=" + x.Value).ToArray());
        }
    }
}