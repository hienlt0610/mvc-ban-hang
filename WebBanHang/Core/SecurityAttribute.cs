using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHang.Core
{
    public class SecurityAttribute : AuthorizeAttribute
    {
        public String AuthUrl { get;set; }
        private String customerAuth = "/Customer/Login";
        private String adminAuth = "/Admin/Auth/Login";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var currentArea = (filterContext.RouteData.DataTokens["area"] ?? "").ToString();
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!String.IsNullOrEmpty(AuthUrl))
                {
                    filterContext.Result = new RedirectResult(AuthUrl);
                    return;
                }
                if(String.IsNullOrEmpty(currentArea))
                {
                    filterContext.Result = new RedirectResult(customerAuth);
                }
                else if(currentArea.Equals("Admin")) 
                {
                    filterContext.Result = new RedirectResult(adminAuth);
                }
                return;
                
            }

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/Account/AccessDenied");
                return;
            }
        }
    }
}