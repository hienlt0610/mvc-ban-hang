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

namespace WebBanHang.Core
{
    public class BaseController : Controller
    {
        protected DbContextRepository Repository { get; set; }
        public BaseController()
        {
            ecommerceEntities entity = new ecommerceEntities();
            Repository = new DbContextRepository(entity);
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