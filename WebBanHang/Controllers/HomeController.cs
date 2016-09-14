using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Core;
using WebBanHang.Core.RepositoryModel;
using System.Web.Configuration;
using System.Configuration;
namespace WebBanHang.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SideBarMenu()
        {
            var groupProduct = Repository.Create<GroupProductRepository>().FetchAll().OrderByDescending(item => item.Priority).ToList();
            return PartialView(groupProduct);
        }
    }
}