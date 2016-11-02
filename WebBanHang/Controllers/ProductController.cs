using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Core.RepositoryModel;

namespace WebBanHang.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var productRepository = Repository.Bind<ProductRepository>();
            var model = productRepository.FindById(id);
            ViewBag.Sale = Repository.Product.BestProductSale();
            return View(model);
        }

        public ActionResult Search(int? group, String q)
        {
            var products = Repository.Product.FetchAll();
            if(group != null && group != 0){
                products = products.Where(p=>p.GroupID == group);
            }
            if (!String.IsNullOrEmpty(q))
            {
                products = products.Where(p=>p.ProductName.ToLower().Contains(q));
            }
            ViewBag.Query = q;
            return View(products);
        }
	}
}