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
            var productRepository = Repository.Create<ProductRepository>();
            var model = productRepository.FindById(id);
            return View(model);
        }
	}
}