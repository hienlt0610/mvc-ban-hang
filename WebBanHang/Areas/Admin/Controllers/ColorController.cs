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
    public class ColorController : AdminBaseController
    {
        //
        // GET: /Admin/Color/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadColor()
        {
            var search = Request.QueryString["search[value]"];
            if (search == null) search = "";
            var colors = Repository.Create<Color>().FetchAll()
                .Where(a => a.ColorName.ToLower().Contains(search))
                .OrderByDescending(c=>c.ColorID);
            List<object> data = new List<object>();
            foreach (var color in colors)
            {
                List<object> attrValue = new List<object>();
                attrValue.Add(color.ColorID);
                attrValue.Add(color.ColorName);
                attrValue.Add(color.HexCode);
                data.Add(attrValue);
            }
            return Content(JsonConvert.SerializeObject(new
            {
                data = data
            }), "application/json");
        }

        public ActionResult ColorInfo(int? id){
            dynamic result = new ExpandoObject();
            if (id == null)
            {
                result.status = false;
                result.message = "Thiếu thông số id màu";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var color = Repository.Create<Color>().FindById(id);
            if (color == null)
            {
                result.status = false;
                result.message = "Màu này không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = true;
            result.data = new
            {
                color_id = color.ColorID,
                color_name = color.ColorName,
                hex_code = color.HexCode
            };
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult InsertColor(Color color)
        {
            dynamic result = new ExpandoObject();
            if (String.IsNullOrEmpty(color.ColorName))
            {
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Chưa nhập tên màu";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            if (String.IsNullOrEmpty(color.HexCode))
            {
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Chưa nhập mã màu";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var repo = Repository.Create<Color>();
            if(repo.FetchAll().Any(c=>c.ColorName.ToLower().Equals(color.ColorName))){
                result.status = "error";
                result.title = "Thêm thất bại";
                result.message = "Tên màu này đã tồn tại trong hệ thống, vui lòng đặt lại tên khác";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            repo.Insert(color);
            repo.SaveChanges();
            result.status = "success";
            result.title = "Thêm thành công!!!";
            result.message = "Chúc mừng bạn đã thêm mới màu sắc thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult DeleteColor(int? id)
        {
            dynamic result = new ExpandoObject();
            if (id == null || id == 0)
            {
                result.status = "error";
                result.title = "Xóa thất bại";
                result.message = "Thiếu mã số màu thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<Color>();
            var color = repo.FindById(id);
            if (color == null)
            {
                result.status = "error";
                result.title = "Xóa thất bại";
                result.message = "Màu này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            repo.Delete(id);
            repo.SaveChanges();
            result.status = "success";
            result.title = "Xóa thành công";
            result.message = "Chúc mừng bạn đã xóa thành công màu này";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult UpdateColor(Color color)
        {
            dynamic result = new ExpandoObject();
            if (String.IsNullOrEmpty(color.ColorName) || color.ColorID == 0 || String.IsNullOrEmpty(color.HexCode))
            {
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "id, Tên màu, mã màu không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<Color>();
            var oldColor = repo.FindById(color.ColorID);
            if (oldColor == null)
            {
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Màu này này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if(repo.FetchAll().Any(c => c.ColorID != color.ColorID && c.ColorName.ToLower().Equals(color.ColorName))){
                result.status = "error";
                result.title = "Chỉnh sửa thất bại";
                result.message = "Tên màu bị trùng với 1 màu đã tồn tại trong hệ thống, vui lòng đặt tên khác";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            oldColor.ColorName = color.ColorName;
            oldColor.HexCode = color.HexCode;
            repo.SaveChanges();

            result.status = "success";
            result.title = "Chỉnh sửa thành công";
            result.message = "Chúc mừng bạn đã thay đổi thành công màu";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
	}
}