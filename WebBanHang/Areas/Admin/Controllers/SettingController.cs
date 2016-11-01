using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class SettingController : AdminBaseController
    {
        //
        // GET: /Admin/Setting/
        public ActionResult Index()
        {
            var config = Repository.Create<Configuration>()
                                        .FetchAll()
                                        .ToDictionary(item => item.ConfigName, item => item.Value);
            var model = new AdminConfigViewModel();
            model.SiteTitle = config["site_title"];
            model.Phone = config["support_phone"];
            model.Email = config["support_email"];
            model.ProductPerPage = Convert.ToInt32(config["product_per_page"]);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdminConfigViewModel model) { 
            if(ModelState.IsValid){
                Repository.Config.UpdateConfig("site_title",model.SiteTitle);
                Repository.Config.UpdateConfig("support_email", model.Email);
                Repository.Config.UpdateConfig("support_phone", model.Phone);
                Repository.Config.UpdateConfig("product_per_page", model.ProductPerPage.ToString());
                Repository.Config.SaveChanges();
                return RedirectToAction("Index","Setting");
            }
            return View(model);
        }
	}
}