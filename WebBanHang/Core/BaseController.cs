using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Core.RepositoryModel;
using WebBanHang.Models;
using WebBanHang.Utils;
using System.Configuration;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Security.Principal;

namespace WebBanHang.Core
{
    public class BaseController : Controller
    {
        protected UnitOfWork Repository { get; set; }
        public BaseController()
        {
            ecommerceEntities entity = new ecommerceEntities();
            Repository = new UnitOfWork(entity);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            //  set viewbag data
            dynamic viewBagData = new ExpandoObject();
            viewBagData.Config = Repository.Create<ConfigRepository>()
                                        .FetchAll()
                                        .ToDictionary(item => item.ConfigName, item => item.Value)
                                        .WithDefaultValue("none");
            ViewBag.Data = viewBagData;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthentication(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if(authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();
                int customerID = js.Deserialize<int>(authTicket.UserData);
                Customer customer = Repository.Customer.FindById(customerID);
                if (customer == null) return;
                var identity = new GenericIdentity(authTicket.Name, "Customer");
                CustomerPrincipal principal = new CustomerPrincipal(identity);
                principal.UserData = customer;
                filterContext.HttpContext.User = principal;
            }
            else
            {
                var principal = new GenericPrincipal(new GenericIdentity(""), null);
                filterContext.HttpContext.User = principal;
            }
        }
	}
}