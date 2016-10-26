using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Models;
using WebBanHang.Utils;
using WebBanHang.ViewModels;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Security]
    public class ProductController : AdminBaseController
    {
        private ecommerceEntities db = new ecommerceEntities();

        // GET: /Admin/Product/
        public ActionResult Index()
        {
            var products = Repository.Product.FetchAll().OrderByDescending(m => m.CreateDate);
            return View(products.ToList());
        }

        // GET: /Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: /Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.GroupProducts = Repository.GroupProduct.FetchAll();
            ViewBag.Colors = Repository.Color.FetchAll().ToList();
            ViewBag.AttrGroup = Repository.Create<AttributeGroup>().FetchAll();
            var model = new AdminProductViewModel();
            model.Active = true;
            return View(model);
        }

        // POST: /Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Detail = HttpUtility.HtmlEncode(model.Detail);
                var product = Mapper.Map<Product>(model);
                product.CreateDate = DateTime.Now;
                if (model.ProductColor.Count > 0)
                    product.UseMultiColor = true;
                else
                    product.UseMultiColor = false;
                Repository.Product.Insert(product);
                Repository.SaveChanges();
                if (model.ProductColor.Count > 0)
                {
                    var quanRepo = Repository.Create<Quantity>();
                    foreach (var pColor in model.ProductColor)
                    {
                        product.ProductColors.Add(new ProductColor() { 
                            ColorID = pColor.ColorID,
                            ProductID = product.ProductID,
                            Stock = pColor.Stock
                        });
                    }
                    Repository.SaveChanges();
                }

                if(model.ProductAttribute.Count > 0){
                    var attrRepo = Repository.Create<ProductAttribute>();
                    foreach (var attr in model.ProductAttribute)
                    {
                        var pAttr = new ProductAttribute()
                        {
                            ProductID = product.ProductID,
                            AttrID = attr.AttrID,
                            Value = attr.Value,
                            Priority = 0
                        };
                        attrRepo.Insert(pAttr);
                    }
                    Repository.SaveChanges();
                }
                
                return RedirectToAction("Index","Product");
                    
            }
            ViewBag.AttrGroup = Repository.Create<AttributeGroup>().FetchAll();
            ViewBag.GroupProducts = Repository.GroupProduct.FetchAll();
            ViewBag.Colors = Repository.Color.FetchAll().ToList();
            return View(model);
        }

        // GET: /Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = Repository.Product.FindById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var pColors = Repository.Create<ProductColor>().FetchAll();
            ViewBag.AttrGroup = Repository.Create<AttributeGroup>().FetchAll();
            ViewBag.GroupProducts = Repository.GroupProduct.FetchAll();
            ViewBag.Colors = Repository.Color.FetchAll().Where(c => !pColors.Any(p=>p.ColorID == c.ColorID && p.ProductID == product.ProductID)).ToList();
            ViewBag.GroupName = product.GroupProduct.GroupName;
            var model = Mapper.Map<Product, AdminProductViewModel>(product);
            model.ProductColor = product.ProductColors.ToList();
            model.ProductAttribute = product.ProductAttributes.ToList();
            model.Detail = HttpUtility.HtmlDecode(model.Detail);
            return View(model);
        }

        // POST: /Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = Repository.Product.FindById(model.ProductID);
                product.ProductName = model.ProductName;
                product.GroupID = model.GroupID;
                product.Detail = HttpUtility.HtmlEncode(model.Detail);
                product.Price = model.Price;
                product.SalePrice = model.SalePrice;
                product.Stock = model.Stock;
                product.Active = model.Active;
                product.UseMultiColor = product.ProductColors.Count > 0 ? true : false;
                Repository.Product.SaveChanges();
                return RedirectToAction("Index","Product");
            }
            ViewBag.AttrGroup = Repository.Create<AttributeGroup>().FetchAll();
            ViewBag.GroupProducts = Repository.GroupProduct.FetchAll();
            ViewBag.Colors = Repository.Color.FetchAll().ToList();

            return View(model);
        }

        // GET: /Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = Repository.Product.FindById(id);
            if (product == null) return Content("error");
            Repository.Product.Delete(product.ProductID);
            Repository.SaveChanges();
            if(Repository.Product.FetchAll().Any(p=>p.ProductID == id))
                return Content("error");
            return Content("success");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetListAttr(int id, int product_id, bool available = false)
        {
            if (id == 0)
            {
                return Content(String.Format("<option value=\"\">Chọn thuộc tính</option>"));
            }
            var attrGroup = Repository.Create<AttributeGroup>().FindById(id);
            var pAttrs = Repository.Create<ProductAttribute>().FetchAll();
            StringBuilder builder = new StringBuilder();
            if (attrGroup == null || (attrGroup !=null && attrGroup.Attributes.Count == 0)) {
                builder.Append(String.Format("<option value=\"\">Không có thuộc tính</option>"));
                return Content(builder.ToString());
            }
            builder.Append(String.Format("<option value=\"\">Chọn thuộc tính</option>"));
            List<WebBanHang.Models.Attribute> availableAttr = attrGroup.Attributes.ToList();
            if (available)
            {
                availableAttr = availableAttr.Where(a => !pAttrs.Any(p=>p.AttrID == a.AttrID && p.ProductID == product_id)).ToList();
            }
            foreach (var attr in availableAttr)
            {
                builder.Append(String.Format("<option value=\"{0}\">{1}</option>",attr.AttrID,attr.AttrName));
            }

            return Content(builder.ToString());
        }

        public ActionResult GetListColor(int id, bool available = false)
        {
            if (id == 0)
            {
                return Content(String.Format("<option value=\"\">Chọn màu</option>"));
            }

            var pColors = Repository.Create<ProductColor>().FetchAll();
            var product = Repository.Product.FindById(id);
            List<Color> colors;
            if (available)
            {
                colors = Repository.Color.FetchAll().Where(c => !pColors.Any(p => p.ColorID == c.ColorID && p.ProductID == product.ProductID)).ToList();
            }
            else
            {
                colors = Repository.Color.FetchAll().ToList();
            }
            StringBuilder builder = new StringBuilder();
            if (colors == null || (colors != null && colors.Count == 0))
            {
                builder.Append(String.Format("<option value=\"\">Không có màu</option>"));
                return Content(builder.ToString());
            }
            builder.Append(String.Format("<option value=\"\">Chọn màu</option>"));
            foreach (var color in colors)
            {
                builder.Append(String.Format("<option value=\"{0}\">{1}</option>", color.ColorID, color.ColorName));
            }

            return Content(builder.ToString());
        }


        public ActionResult LoadAttr(int id) {
            var attrList = Repository.Create<ProductAttribute>().FetchAll().Where(a=>a.ProductID == id);
            var data = new List<object>();
            foreach(var attr in attrList){
                var dataAttr = new List<object>();
                dataAttr.Add(attr.Attribute.AttrName);
                dataAttr.Add(attr.Value);
                dataAttr.Add(attr.AttrID);
                data.Add(dataAttr);
            }
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadColor(int id)
        {
            var product = Repository.Product.FindById(id);
            var data = new List<object>();
            foreach (var color in product.ProductColors)
            {
                var dataColor = new List<object>();
                dataColor.Add(color.Color.ColorName);
                dataColor.Add(color.Stock);
                dataColor.Add(color.ColorID);
                data.Add(dataColor);
            }
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadImage(int id)
        {
            var product = Repository.Product.FindById(id);
            var data = new List<object>();
            foreach (var img in product.ImageProducts)
            {
                var dataImage = new List<object>();
                dataImage.Add(img.ImagePath);
                dataImage.Add(img.ImageID);
                data.Add(dataImage);
            }
            return Json(new
            {
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAttr(int? id, int? attr_id) {
            var repo = Repository.Create<ProductAttribute>();
            if (id == null || attr_id == null) return Content("error");
            String status = "";
            if (repo.Delete(id, attr_id))
                status = "success";
            else
                status = "error";
            repo.SaveChanges();
            return Content(status);
        }

        public ActionResult DeleteImage(ImageProduct pImg)
        {
            var repo = Repository.Create<ImageProduct>();
            if (pImg.ImageID == 0 || pImg.ProductID == 0) return Content("error");
            String status = "";
            if (repo.Delete(pImg.ImageID,pImg.ProductID))
                status = "success";
            else
                status = "error";
            repo.SaveChanges();
            return Content(status);
        }

        [HttpPost]
        public ActionResult InsertAttr(ProductAttribute pAttr)
        {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (pAttr.ProductID == 0 || pAttr.AttrID == 0 || String.IsNullOrEmpty(pAttr.Value)) {
                result.status = false;
                result.message = "Thiếu thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var product = Repository.Product.FindById(pAttr.ProductID);
            var attr = Repository.Create<WebBanHang.Models.Attribute>().FindById(pAttr.AttrID);
            if(product == null){
                result.status = false;
                result.message = "Không tồn tại sản phẩm";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if (attr == null)
            {
                result.status = false;
                result.message = "Không tồn thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var existpAttr = product.ProductAttributes.Any(p=>p.AttrID == pAttr.AttrID);
            if (existpAttr)
            {
                result.status = false;
                result.message = "Sản phẩm đã có thuộc tính này";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            pAttr.Priority = 0;
            product.ProductAttributes.Add(pAttr);
            if (Repository.SaveChanges() == 0)
            {
                result.status = false;
                result.message = "Lỗi xảy ra";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            result.status = true;
            result.message = "Thêm thuộc tính thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult InsertColor(ProductColor pColorBind)
        {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (pColorBind.ColorID == 0 || pColorBind.ProductID == 0)
            {
                result.status = false;
                result.message = "Thiếu thông tin đầu vào";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var product = Repository.Product.FindById(pColorBind.ProductID);
            var color = Repository.Color.FindById(pColorBind.ColorID);
            if(product == null){
                result.status = false;
                result.message = "Sản phẩm không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if(color == null){
                result.status = false;
                result.message = "Màu này không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if(product.ProductColors.Any(p=>p.ColorID == pColorBind.ColorID))
            {
                result.status = false;
                result.message = "Màu này đã được thêm trước đó rồi";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            product.ProductColors.Add(pColorBind);
            if (Repository.SaveChanges() == 0)
            {
                result.status = false;
                result.message = "Lỗi xảy ra";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            result.status = true;
            result.message = "Thêm thuộc tính thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult DeleteColor(ProductColor pColorBind)
        {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (pColorBind.ColorID == 0 || pColorBind.ProductID == 0)
            {
                result.status = false;
                result.message = "Thiếu thông tin đầu vào";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var product = Repository.Product.FindById(pColorBind.ProductID);
            var color = Repository.Color.FindById(pColorBind.ColorID);
            if (product == null)
            {
                result.status = false;
                result.message = "Sản phẩm không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if (color == null)
            {
                result.status = false;
                result.message = "Màu này không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if (!product.ProductColors.Any(p => p.ColorID == pColorBind.ColorID))
            {
                result.status = false;
                result.message = "Sản phẩm không có màu này nên không thể xóa";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            Repository.Create<ProductColor>().Delete(pColorBind.ProductID, pColorBind.ColorID);
            if (Repository.Product.SaveChanges() == 0)
            {
                result.status = false;
                result.message = "Lỗi không thể xóa được";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            result.status = true;
            result.message = "Xóa màu thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");

        }

        public ActionResult UpdateAttr(ProductAttribute pAttr)
        {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (pAttr.ProductID == 0 || pAttr.AttrID == 0 || String.IsNullOrEmpty(pAttr.Value))
            {
                result.status = false;
                result.message = "Thiếu thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var product = Repository.Product.FindById(pAttr.ProductID);
            var attr = Repository.Create<WebBanHang.Models.Attribute>().FindById(pAttr.AttrID);
            if (product == null)
            {
                result.status = false;
                result.message = "Không tồn tại sản phẩm";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if (attr == null)
            {
                result.status = false;
                result.message = "Không tồn thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var existpAttr = product.ProductAttributes.FirstOrDefault(p => p.AttrID == pAttr.AttrID);
            if (existpAttr == null)
            {
                result.status = false;
                result.message = "Sản phẩm không có thuộc tính này rồi";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            existpAttr.Value = pAttr.Value;

            Repository.SaveChanges();
            result.status = true;
            result.message = "Chỉnh sửa thuộc tính thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult UpdateColor(ProductColor pColor)
        {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (pColor.ProductID == 0 || pColor.ColorID == 0)
            {
                result.status = false;
                result.message = "Thiếu thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var product = Repository.Product.FindById(pColor.ProductID);
            var color = Repository.Color.FindById(pColor.ColorID);
            if (product == null)
            {
                result.status = false;
                result.message = "Không tồn tại sản phẩm";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            if (color == null)
            {
                result.status = false;
                result.message = "Không tồn thuộc tính";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var existpColor = product.ProductColors.FirstOrDefault(c => c.ColorID == pColor.ColorID);
            if (existpColor == null)
            {
                result.status = false;
                result.message = "Sản phẩm không có màu sắc này rồi";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            existpColor.Stock = pColor.Stock;

            Repository.SaveChanges();
            result.status = true;
            result.message = "Chỉnh sửa màu thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult GetProductAttr(int? product_id, int? attr_id) {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (product_id == null || attr_id == null)
            {
                result.status = false;
                result.message = "Thiếu dữ liệu đầu vào";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<ProductAttribute>();
            var pAttr = repo.FindById(product_id, attr_id);
            if(pAttr == null){
                result.status = false;
                result.message = "Thuộc tính của sản phẩm này không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = true;
            result.message = "";
            result.data = new
            {
                product_id = pAttr.ProductID,
                attr_id = pAttr.AttrID,
                attr_group = pAttr.Attribute.AttriGroupID,
                attr_text = pAttr.Attribute.AttrName,
                attr_value = pAttr.Value
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult GetProductColor(int? product_id, int? color_id)
        {
            dynamic result = new ExpandoObject();
            result.status = true;
            result.message = "";
            if (product_id == null || color_id == null)
            {
                result.status = false;
                result.message = "Thiếu dữ liệu đầu vào";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var repo = Repository.Create<ProductColor>();
            var pColor = repo.FindById(product_id, color_id);
            if (pColor == null)
            {
                result.status = false;
                result.message = "Màu của sản phẩm này không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = true;
            result.message = "";
            result.data = new
            {
                product_id = pColor.ProductID,
                color_id = pColor.ColorID,
                color_name = pColor.Color.ColorName,
                stock = pColor.Stock
            };

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult ImageUpload(int product_id)
        {
            dynamic result = new ExpandoObject();
            result.status = "error";
            result.message = "";
            HttpFileCollectionBase hfc = Request.Files;
            var product = Repository.Product.FindById(product_id);
            for (int i = 0; i < hfc.Count; i++)
            {
                HttpPostedFileBase file = hfc[i];
                if(file != null || file.ContentLength > 0){
                    var currDate = DateTime.Now;
                    var fileName = StringUtils.GenerateID()+"_"+file.FileName;
                    var folderSave = Server.MapPath("~/Uploads/"+currDate.Year+"/"+currDate.Month+"/"+currDate.Day);
                    bool folderExists = Directory.Exists(folderSave);
                    if (!folderExists)
                        Directory.CreateDirectory(folderSave);
                    var fileSave = Path.Combine(folderSave, fileName);
                    file.SaveAs(fileSave);
                    result.status = "success";
                    result.message = "Upload thành công";
                    product.ImageProducts.Add(new ImageProduct() { 
                        ImagePath = fileSave.Replace(Server.MapPath("~/"), "/").Replace(@"\", "/"),
                        Caption = file.FileName
                    });
                    Repository.SaveChanges();
                    break;
                }
                else
                {
                    result.status = "error";
                    result.message = "Lỗi không upload được";
                    break;
                }
            }
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
    }
}
