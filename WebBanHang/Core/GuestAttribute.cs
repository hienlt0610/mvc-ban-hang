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
        public String Action { get; set; }
        public String Controller { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if(user.Identity.IsAuthenticated)
            {
                if (Action == null)
                    Action = "Index";
                if(Controller == null)
                    Controller = "Home";
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary 
                { 
                    { "controller", Controller }, 
                    { "action", Action } 
                });
            }
        }
    }
}