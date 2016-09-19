using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Utils
{
    public static class HtmlExtension
    {
        public static String FormatCurrency(this HtmlHelper helper, long? price)
        {
            return price.GetValueOrDefault(0).ToString("#,##0");
        }

        public static String FormatCurrency(this HtmlHelper helper, long price)
        {
            return price.ToString("#,##0");
        }

        public static String FormatCurrency(this HtmlHelper helper, int price)
        {
            return price.ToString("#,##0");
        }

        public static String FormatCurrency(this HtmlHelper helper, int? price)
        {
            return price.GetValueOrDefault(0).ToString("#,##0");
        }
    }
}