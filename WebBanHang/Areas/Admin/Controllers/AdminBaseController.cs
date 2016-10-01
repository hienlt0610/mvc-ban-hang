using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebBanHang.Core;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class AdminBaseController : BaseController
    {
        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName+"_ADMIN"];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();
                int userID = js.Deserialize<int>(authTicket.UserData);
                User user = Repository.User.FindById(userID);
                if (user == null) return;
                var identity = new GenericIdentity(authTicket.Name, "User");
                var principal = new UserPrincipal(identity);
                principal.UserData = user;
                filterContext.HttpContext.User = principal;
            }
            else
            {
                var principal = new GenericPrincipal(new GenericIdentity(String.Empty), null);
                filterContext.HttpContext.User = principal;
            }
        }
	}
}