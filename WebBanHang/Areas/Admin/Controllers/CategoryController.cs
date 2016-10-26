using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class CategoryController : AdminBaseController
    {
        //
        // GET: /Admin/Category/
        public ActionResult Index()
        {
            var groups = Repository.GroupProduct.FetchAll().Where(g=>g.ParentGroupID == null);
            ViewBag.Groups = groups;
            return View();
        }

        public ActionResult Create()
        {
            var model = new AdminGroupProductViewModel();
            ViewBag.Groups = Repository.GroupProduct.FetchAll().Where(g=>g.ParentGroupID == null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminGroupProductViewModel model)
        {
            if(ModelState.IsValid){
                var group = Mapper.Map<GroupProduct>(model);
                Repository.GroupProduct.Insert(group);
                Repository.GroupProduct.SaveChanges();
                if (group.GroupID != 0)
                {
                    return RedirectToAction("Index","Category");
                }
            }
            ViewBag.Groups = Repository.GroupProduct.FetchAll().Where(g => g.ParentGroupID == null);
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            var group = Repository.GroupProduct.FindById(id);
            if (group == null) return HttpNotFound();

            var model = Mapper.Map<AdminGroupProductViewModel>(group);
            ViewBag.Groups = Repository.GroupProduct.FetchAll().Where(g => g.ParentGroupID == null);
            return View(model);
        }

        public ActionResult LoadCategory(int start, int length)
        {
            var search = Request.QueryString["search[value]"].ToString();
            var parentGroup = Request.QueryString["columns[2][search][value]"].ToString();
            var cates = Repository.GroupProduct.FetchAll().Where(c => c.GroupName.Contains(search));
            if(!String.IsNullOrEmpty(parentGroup) && !parentGroup.Equals("all")){
                int? groupID = (int.Parse(parentGroup) as int?);
                cates = cates.Where(c=>c.ParentGroupID == groupID);
            }
            cates = cates.OrderByDescending(c=>c.Priority);
            var catePaging = cates.Skip(start).Take(length);
            List<object> data = new List<object>();
            foreach (var cate in catePaging)
            {
                var row = new List<object>();
                row.Add(cate.GroupID.ToString());
                row.Add(cate.GroupName);
                row.Add(cate.ParentGroupID);
                row.Add(cate.Icon);
                row.Add(cate.Priority);
                data.Add(row);
            }
            var result = new
            {
                draw = Request.QueryString["draw"],
                recordsTotal = cates.Count(),
                recordsFiltered = cates.Count(),
                search = search,
                data = data
            };
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult Delete(int? id, bool confirm = false)
        {
            dynamic result = new ExpandoObject();
            if (id == null)
            {
                result.status = "error";
                result.title = "Lỗi";
                result.message = "Không có mã id nhóm";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }

            var group = Repository.GroupProduct.FindById(id);
            if (group == null)
            {
                result.status = "error";
                result.title = "Lỗi";
                result.message = "Nhóm này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if (group.Products.Count > 0 && !confirm)
            {
                result.status = "warning";
                result.title = "Cảnh báo";
                result.message = "Nhóm này chứa nhiều sản phẩm, khi xóa sẽ mất hết sản phẩm, hãy cân nhắc trước khi xóa";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            Repository.GroupProduct.Delete(id);
            Repository.SaveChanges();
            if(Repository.GroupProduct.FetchAll().Any(g=>g.GroupID == id)){
                result.status = "error";
                result.title = "Lỗi";
                result.message = "Đã có lỗi xảy ra, không thể xóa được";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            result.status = "success";
            result.title = "Thành công";
            result.message = "Chúc mừng bạn đã xóa thành công";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
	}
}