using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Models;
using WebBanHang.Utils;

namespace WebBanHang.Controllers
{
    public class CartController : BaseController
    {
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CartTotal()
        {
            return PartialView(Cart);
        }

        [HttpPost]
        public ActionResult AddCart(int? id, int? color, int quantity)
        {
            if (id == null) return HttpNotFound("Id bị trống");
            Product product = Repository.Product.FindById(id);
            Color colorItem = Repository.Color.FindById(color);
            if (product == null) return HttpNotFound("Không tồn tại sản phẩm");
            Cart.AddItem(product, colorItem, quantity);
            return PartialView("ShoppingCartView");
        }

        [HttpPost]
        public ActionResult RemoveCart(int? id, int? color)
        {
            if (id == null) return HttpNotFound("Id bị trống");
            Product product = Repository.Product.FindById(id);
            Color colorItem = Repository.Color.FindById(color);
            if (product == null) return HttpNotFound("Không tồn tại sản phẩm");
            Cart.Remove(product, colorItem);
            return PartialView("ShoppingCartView");
        }

        [HttpPost]
        public ActionResult UpdateCart(int? id, int? color, int quantity)
        {
            if (id == null) return HttpNotFound("Id empty");
            Product product = Repository.Product.FindById(id);
            Color colorItem = Repository.Color.FindById(color);
            if (product == null) return HttpNotFound("Item not found");
            var item = Cart.Update(product,colorItem,quantity);
            if(item != null)
                return Content(HtmlExtension.FormatCurrency(item.TotalPrice) +" đ");
            return HttpNotFound();
        }


        public ActionResult GetListColor(int? id)
        {
            dynamic result = new ExpandoObject();
            var colors = new List<object>();
            result.status = "";
            result.message = "";
            result.count = colors.Count;
            result.colors = colors;


            if (id == null)
            {
                result.status = "error";
                result.message = "ID bị trống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            Product product = Repository.Product.FindById(id);
            if (product == null)
            {
                result.status = "error";
                result.message = "Sản phẩm không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            result.status = "OK";
            result.count = product.Colors.Count;
            foreach(var color in product.Colors){
                colors.Add(new
                {
                    color_id = color.ColorID,
                    color_name = color.ColorName,
                    hex_code = color.HexCode
                });
            }
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult ShoppingCartView()
        {
            return PartialView();
        }
	}
}