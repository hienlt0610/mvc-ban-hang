using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Core
{
    public class MyRazorViewEngine : RazorViewEngine
    {
        public MyRazorViewEngine()
            : base()
        {
            AreaViewLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
        };

            AreaMasterLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
        };

            AreaPartialViewLocationFormats = new[] {
            "~/Areas/{2}/Views/{1}/{0}.cshtml",
            "~/Areas/{2}/Views/{1}/{0}.vbhtml",
            "~/Areas/{2}/Views/Shared/{0}.cshtml",
            "~/Areas/{2}/Views/Shared/{0}.vbhtml"
        };

            ViewLocationFormats = new[] {
            "~/Views/{1}/{0}.cshtml",
            "~/Views/{1}/{0}.vbhtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Views/Shared/{0}.vbhtml"
        };

            MasterLocationFormats = new[] {
            "~/Views/{1}/{0}.cshtml",
            "~/Views/{1}/{0}.vbhtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Views/Shared/{0}.vbhtml"
        };

            PartialViewLocationFormats = new[] {
            "~/Views/{1}/{0}.cshtml",
            "~/Views/{1}/{0}.vbhtml",
            "~/Views/Shared/{0}.cshtml",
            "~/Views/Shared/{0}.vbhtml"
        };
        }
    }
}