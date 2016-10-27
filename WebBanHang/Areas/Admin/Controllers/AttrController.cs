using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Utils;
using WebBanHang.Models;
using System.Dynamic;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class AttrController : AdminBaseController
    {
        //
        // GET: /Admin/Attr/
        public ActionResult Index()
        {
            ViewBag.GroupAttr = Repository.Create<AttributeGroup>().FetchAll();
            return View();
        }

        public ActionResult LoadAttr()
        {
            var search = Request.QueryString["search[value]"];
            var attrs = Repository.Create<WebBanHang.Models.Attribute>()
                                    .FetchAll()
                                    .Where(a => a.AttrName.ToLower().Contains(search))
                                    .OrderBy(a=>a.AttriGroupID);
            List<object> data = new List<object>();
            foreach (var attr in attrs)
            {
                List<object> attrValue = new List<object>();
                attrValue.Add(attr.AttrID);
                attrValue.Add(attr.AttrName);
                //if(attr.AttriGroupID == null)
                //    attrValue.Add("Nhóm chung");
                //else
                //    attrValue.Add(attr.AttributeGroup.AttrGroupName);
                attrValue.Add(new { 
                    group_id = (attr.AttriGroupID == null) ? 0 : attr.AttriGroupID,
                    group_name = (attr.AttriGroupID == null)?"Nhóm chung":attr.AttributeGroup.AttrGroupName
                });
                data.Add(attrValue);    
            }
            return Content(JsonConvert.SerializeObject(new
            {
                data = data
            }), "application/json");
        }

        [HttpPost]
        public ActionResult InsertAttr(WebBanHang.Models.Attribute attr)
        {
            dynamic result = new ExpandoObject();
            if(String.IsNullOrEmpty(attr.AttrName)){
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Thiếu thông số";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<WebBanHang.Models.Attribute>();
            if(repo.FetchAll().Any(a=>a.AttrName.ToLower().Contains(attr.AttrName))){
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Thuộc tính này đã tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            repo.Insert(attr);
            repo.SaveChanges();
            result.status = "success";
            result.title = "Thêm thành công";
            result.message = "Chúc mừng bạn đã thêm mới thuộc tính thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult DeleteAttr(int? id)
        {
            dynamic result = new ExpandoObject();
            if (id == null || id == 0)
            {
                result.status = "error";
                result.title = "Xóa thất bại";
                result.message = "Thiếu mã thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<WebBanHang.Models.Attribute>();
            var attr = repo.FindById(id);
            if (attr == null)
            {
                result.status = "error";
                result.title = "Xóa thất bại";
                result.message = "Thuộc tính này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            repo.Delete(id);
            repo.SaveChanges();
            result.status = "success";
            result.title = "Xóa thành công";
            result.message = "Chúc mừng bạn đã xóa thành công thuộc tính này!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult AttrInfo(int? id)
        {
            dynamic result = new ExpandoObject();
            if (id == null)
            {
                result.status = false;
                result.message = "Thiếu thông số";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var attr = Repository.Create<WebBanHang.Models.Attribute>().FindById(id);
            if (attr == null)
            {
                result.status = false;
                result.message = "Thuộc tính không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = true;
            result.data = new {
                attr_id = attr.AttrID,
                attr_name = attr.AttrName,
                attr_group_id = attr.AttriGroupID,
                attr_group_name = (attr.AttriGroupID == null) ? "" : attr.AttributeGroup.AttrGroupName
            };
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult UpdateAttr(WebBanHang.Models.Attribute attr)
        {
            dynamic result = new ExpandoObject();

            if(attr.AttrID == 0){
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Thiếu mã thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            if (String.IsNullOrEmpty(attr.AttrName))
            {
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Tên thuộc tính không được rỗng";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var repo = Repository.Create<WebBanHang.Models.Attribute>();
            var oldAttr = repo.FindById(attr.AttrID);
            if(oldAttr == null){
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Thuộc tính này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            if (repo.FetchAll().Any(a => a.AttrID != attr.AttrID && a.AttrName.ToLower().Contains(attr.AttrName)))
            {
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Tên thuộc tính đã tồn tại, vui lòng đặt tên thuộc tính khác";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            oldAttr.AttrName = attr.AttrName;
            oldAttr.AttriGroupID = attr.AttriGroupID;
            repo.SaveChanges();
            result.status = "success";
            result.title = "Chỉnh sửa thành công";
            result.message = "Chúc mừng bạn đã thay đổi thông tin thuộc tính thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
	}
}