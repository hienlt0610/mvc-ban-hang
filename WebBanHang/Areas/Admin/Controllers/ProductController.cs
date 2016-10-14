using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            ViewBag.GroupID = new SelectList(db.GroupProducts, "GroupID", "GroupName");
            return View();
        }

        // POST: /Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ProductID,ProductName,Detail,Price,SalePrice,Stock,GroupID,UseMultiColor,CreateDate,Active")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupID = new SelectList(db.GroupProducts, "GroupID", "GroupName", product.GroupID);
            return View(product);
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
            ViewBag.GroupID = new SelectList(db.GroupProducts, "GroupID", "GroupName", product.GroupID);
            ViewBag.GroupProducts = Repository.GroupProduct.FetchAll();
            var model = Mapper.Map<Product, AdminProductViewModel>(product);
            return View(model);
        }

        // POST: /Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminProductViewModel product)
        {
            foreach (string key in Request.Form.Keys)
            {
                Response.Write(key + ": " + Request.Form[key] + "<br/>");
            }
            return new EmptyResult();
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = new SelectList(db.GroupProducts, "GroupID", "GroupName", product.GroupID);
            return View(product);
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
    }
}
