using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class AttrGroupController : AdminBaseController
    {
        //
        // GET: /Admin/AttrGroup/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult LoadGroupAttr()
        {
            var search = Request.QueryString["search[value]"];
            var attrGroup = Repository.Create<AttributeGroup>().FetchAll().Where(a => a.AttrGroupName.Contains(search));
            List<object> data = new List<object>();
            foreach(var group in attrGroup){
                List<object> attrValue = new List<object>();
                attrValue.Add(group.AttrGroupID);
                attrValue.Add(group.AttrGroupName);
                data.Add(attrValue);
            }
            return Content(JsonConvert.SerializeObject(new { 
                data = data
            }),"application/json");
        }

        [HttpPost]
        public ActionResult InsertGroupAttr(AttributeGroup group)
        {
            dynamic result = new ExpandoObject();
            if (String.IsNullOrEmpty(group.AttrGroupName))
            {
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Tên nhóm thuộc tính không được để trống";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }
            var repo = Repository.Create<AttributeGroup>();
            var isExist = repo.FetchAll().Any(a => a.AttrGroupName.Equals(group.AttrGroupName,StringComparison.OrdinalIgnoreCase));
            if(isExist){
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Nhóm này đã tồn tại trong hệ thống, vui lòng chọn tên khác";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            repo.Insert(group);
            repo.SaveChanges();
            if (group.AttrGroupID == 0)
            {
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Đã có lỗi xảy ra, không thể thêm mới sản phẩm";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = "success";
            result.title = "Thêm mới thành công";
            result.message = "Thêm nhóm thuộc tính thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult AttrGroupInfo(int? id)
        {
            dynamic result = new ExpandoObject();
            if(id == null)
            {
                result.status = false;
                result.message = "Thiếu thông số";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var attrGroup = Repository.Create<AttributeGroup>().FindById(id);
            if (attrGroup == null)
            {
                result.status = false;
                result.message = "Nhóm thuộc tính không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = true;
            result.data = new {
                attr_group_id = attrGroup.AttrGroupID,
                attr_group_name = attrGroup.AttrGroupName
            };
            return Content(JsonConvert.SerializeObject(result),"application/json");
        }

        [HttpPost]
        public ActionResult UpdateGroupAttr(AttributeGroup group)
        {
            dynamic result = new ExpandoObject();
            if (String.IsNullOrEmpty(group.AttrGroupName) || group.AttrGroupID == 0)
            {
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Tên nhóm thuộc tính hoặc mã nhóm thuộc tính không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<AttributeGroup>();
            var oldGroup = repo.FindById(group.AttrGroupID);
            if(oldGroup == null){
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Nhóm thuộc tính này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            oldGroup.AttrGroupName = group.AttrGroupName;
            repo.SaveChanges();
            result.status = "success";
            result.title = "Chỉnh sửa thành công";
            result.message = "Chúc mừng bạn đã thay đổi thành công thông tin nhóm thuộc tính";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult DeleteAttrGroup(int? id)
        {
            dynamic result = new ExpandoObject();
            if (id == null || id == 0)
            {
                result.status = "error";
                result.title = "Xóa thất bại";
                result.message = "Thiếu mã nhóm thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<AttributeGroup>();
            var group = repo.FindById(id);
            if (group == null)
            {
                result.status = "error";
                result.title = "Xóa thất bại";
                result.message = "Nhóm thuộc tính này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            repo.Delete(id);
            repo.SaveChanges();
            result.status = "success";
            result.title = "Xóa thành công";
            result.message = "Chúc mừng bạn đã xóa thành công thông tin nhóm thuộc tính";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
	}

}