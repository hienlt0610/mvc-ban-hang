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
    public class HomeController : AdminBaseController
    {
        [Security]
        public ActionResult Index()
        {
            ViewBag.Orders = Repository.Order.FetchAll();
            ViewBag.Products = Repository.Product.FetchAll();
            ViewBag.Customers = Repository.Customer.FetchAll();
            return View("Index");
        }

	}
}