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
	}
}