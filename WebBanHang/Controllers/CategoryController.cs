using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Utils;
using WebBanHang.Core.RepositoryModel;
using PagedList;

namespace WebBanHang.Controllers
{
    public class CategoryController : BaseController
    {
        GroupProductRepository groupRepository;
        public CategoryController()
        {
            groupRepository = Repository.Create<GroupProductRepository>();
        }
        //
        // GET: /Category/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Error404", "Error");
            var model = groupRepository.FindById(id);
            if (model == null) return RedirectToAction("Error404", "Error");
            return View(model);
        }

        public ActionResult ListGroupProduct(int id)
        {
            dynamic model = new ExpandoObject();
            model.GroupProducts = groupRepository.GetListSubGroups(id);
            model.CurrentGroup = groupRepository.FindById(id);
            return PartialView(model);
        }

        public ActionResult ShowProductInCategory(int id)
        {
            var model = groupRepository.GetProductInGroups(id);
            int page = 1;
            if (Request.Params["page"] != null)
            {
                page = Request.Params["page"].ToIntWithDef(1);
            }
            var config = Repository.Create<ConfigRepository>().FindById("product_per_page");
            var pageSize = config.Value.ToIntWithDef(1);
            if (pageSize < 1) pageSize = 1;
            ViewData["groupID"] = id;
            return PartialView(model.ToPagedList(page, pageSize));
        }
	}
}