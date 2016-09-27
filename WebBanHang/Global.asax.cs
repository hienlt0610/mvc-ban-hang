using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using WebBanHang.Core;
using WebBanHang.Models;

namespace WebBanHang
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();
                var dataToken = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
                String area = dataToken != null ? dataToken.ToString() : "";
                if (!area.Equals("Admin"))
                {
                    Customer customer = js.Deserialize<Customer>(authTicket.UserData);
                    var identity = new GenericIdentity(authTicket.Name, "Forms");
                    CustomerPrincipal principal = new CustomerPrincipal(identity);
                    principal.UserData = customer;
                    HttpContext.Current.User = principal;
                }
            }
        }
    }
}
