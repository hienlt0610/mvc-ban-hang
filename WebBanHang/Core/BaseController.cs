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

namespace WebBanHang.Core
{
    public class BaseController : Controller
    {
        protected DbContextRepository Repository { get; set; }
        public BaseController()
        {
            //ecommerceEntities entity = new ecommerceEntities();
            String connectionString = "Data Source=(local);Initial Catalog=ecommerce;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
            String entityConnStr = String.Format("metadata=res://*/Models.Ecommerce.csdl|res://*/Models.Ecommerce.ssdl|res://*/Models.Ecommerce.msl;provider=System.Data.SqlClient;provider connection string='{0}'",connectionString);
            Repository = new DbContextRepository(new WebDbContext(entityConnStr));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Optional: Work only for GET request
            if (filterContext.RequestContext.HttpContext.Request.RequestType != "GET")
                return;

            // Optional: Do not work with AjaxRequests
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                return;
            //  set viewbag data
            dynamic viewBagData = new ExpandoObject();
            viewBagData.Config = Repository.Create<ConfigRepository>()
                                        .FetchAll()
                                        .ToDictionary(item => item.ConfigName, item => item.Value)
                                        .WithDefaultValue("none");
            ViewBag.Data = viewBagData;
            base.OnActionExecuting(filterContext);
        }
	}
}