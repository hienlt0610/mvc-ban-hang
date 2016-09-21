using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Core.RepositoryModel;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ProductManagerController : BaseController
    {
        //
        // GET: /ProductManager/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            var groupRepo = Repository.Create<GroupProductRepository>();
            ViewBag.GroupProducts = groupRepo.FetchAll();
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form, Product product)
        {
            StringBuilder builder = new StringBuilder();
            product.CreateDate = DateTime.Now;
            var productRepo = Repository.Create<ProductRepository>();
            Product insert = productRepo.Insert(product);
            productRepo.SaveChanges();
            return RedirectToAction("Add");
        }
	}
}