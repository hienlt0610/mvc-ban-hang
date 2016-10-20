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
using System.Dynamic;

namespace WebBanHang.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var productRes = Repository.Bind<ProductRepository>();
            var groupRes = Repository.Bind<GroupProductRepository>(); 
            dynamic model = new ExpandoObject();
            model.NewProduct = productRes.GetNewProduct(5);
            model.GroupProducts = groupRes.GetTopGroupProducts();
            return View(model);
        }

        public ActionResult SideBarMenu()
        {
            var groupProduct = Repository.Bind<GroupProductRepository>()
                            .FetchAll()
                            .Where(item => item.ParentGroupID == null)
                            .OrderByDescending(item => item.Priority)
                            .ToList();
            return PartialView(groupProduct);
        }

        public ActionResult ShowGroupItem(int id)
        {
            var groupRes = Repository.Bind<GroupProductRepository>();
            dynamic model = new ExpandoObject();
            List<Product> products = groupRes.GetProductInGroups(id);
            model.Products = products;
            model.Group = groupRes.FindById(id);
            if (products.Count == 0) return Content("");
            return PartialView(model);
        }
    }
}