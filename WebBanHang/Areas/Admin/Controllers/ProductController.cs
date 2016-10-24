using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Models;
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
                if (model.Quantity.Count > 0)
                    product.UseMultiColor = true;
                else
                    product.UseMultiColor = false;
                Repository.Product.Insert(product);
                Repository.SaveChanges();
                if (model.Quantity.Count > 0)
                {
                    var quanRepo = Repository.Create<Quantity>();
                    foreach (var quantity in model.Quantity)
                    {
                        //Add Product Color
                        var color = Repository.Color.FindById(quantity.ColorID);
                        product.Colors.Add(color);
                        //Add Quantity with color
                        quanRepo.Insert(new Quantity() { 
                            ColorID = quantity.ColorID,
                            ProductID = product.ProductID,
                            Stock = quantity.Stock
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
            ViewBag.AttrGroup = Repository.Create<AttributeGroup>().FetchAll();
            ViewBag.GroupProducts = Repository.GroupProduct.FetchAll();
            ViewBag.Colors = Repository.Color.FetchAll().ToList();
            var model = Mapper.Map<Product, AdminProductViewModel>(product);
            model.Quantity = product.Quantities.ToList();
            model.ProductAttribute = product.ProductAttributes.ToList();
            return View(model);
        }

        // POST: /Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminProductViewModel model)
        {
            model.Detail = HttpUtility.HtmlEncode(model.Detail);
            if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(model));
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetListAttr(int id)
        {
            if (id == 0)
            {
                return Content(String.Format("<option value=\"\">Chọn thuộc tính</option>"));
            }
            var attrGroup = Repository.Create<AttributeGroup>().FindById(id);
            StringBuilder builder = new StringBuilder();
            if (attrGroup == null || (attrGroup !=null && attrGroup.Attributes.Count == 0)) {
                builder.Append(String.Format("<option value=\"\">Không có thuộc tính</option>"));
                return Content(builder.ToString());
            }
            builder.Append(String.Format("<option value=\"\">Chọn thuộc tính</option>"));
            foreach(var attr in attrGroup.Attributes){
                builder.Append(String.Format("<option value=\"{0}\">{1}</option>",attr.AttrID,attr.AttrName));
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
    }
}
