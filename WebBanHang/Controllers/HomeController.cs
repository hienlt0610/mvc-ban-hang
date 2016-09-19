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
            var productRes = Repository.Create<ProductRespository>();
            var groupRes = Repository.Create<GroupProductRepository>(); 
            dynamic model = new ExpandoObject();
            model.NewProduct = productRes.GetNewProduct(5);
            model.GroupProducts = groupRes.GetTopGroupProduct();
            return View(model);
        }

        public ActionResult SideBarMenu()
        {
            var groupProduct = Repository.Create<GroupProductRepository>()
                            .FetchAll()
                            .Where(item => item.ParentGroupID == null)
                            .OrderByDescending(item => item.Priority)
                            .ToList();
            return PartialView(groupProduct);
        }

        public ActionResult ShowGroupItem(int id)
        {
            var groupRes = Repository.Create<GroupProductRepository>();
            dynamic model = new ExpandoObject();
            List<Product> products = groupRes.GetProductInGroup(id);
            model.Products = products;
            model.Group = groupRes.FindById(id);
            if (products.Count == 0) return Content("");
            return PartialView(model);
        }
    }
}