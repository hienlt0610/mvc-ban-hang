using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanHang.Core
{
    public class OnlyGuestAttribute : ActionFilterAttribute
    {
        public String AuthUrl { get; set; }
        private String customerAuth = "/Home";
        private String adminAuth = "/Admin/Home";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentArea = (filterContext.RouteData.DataTokens["area"] ?? "").ToString();
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(AuthUrl))
                {
                    filterContext.Result = new RedirectResult(AuthUrl);
                    return;
                }
                if (String.IsNullOrEmpty(currentArea))
                {
                    filterContext.Result = new RedirectResult(customerAuth);
                }
                else if (currentArea.Equals("Admin"))
                {
                    filterContext.Result = new RedirectResult(adminAuth);
                }
                return;

            }
        }
    }
}