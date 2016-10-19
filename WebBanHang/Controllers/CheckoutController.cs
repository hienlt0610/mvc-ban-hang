using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.ViewModels;

namespace WebBanHang.Controllers
{
    [Authorize]
    public class CheckoutController : BaseController
    {
        //
        // GET: /Checkout/
        public ActionResult Index()
        {
            return RedirectToAction("Shipping");
        }

        [HttpGet]
        public ActionResult Shipping()
        {
            if (Cart.GetCount() == 0)
                return RedirectToAction("Index", "Cart");
            ViewData["Provinces"] = Repository.Province.FetchAll().OrderBy(i=>i.Type+" "+i.ProvinceName).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Shipping(ShippingViewModel model)
        {
            model.Province = Repository.Province.FindById(model.ProvinceID);
            model.District = Repository.District.FindById(model.DistrictID);
            model.Ward = Repository.Ward.FindById(model.WardID);
            TempData["Ship"] = model;
            return RedirectToAction("Payment","Checkout");
        }

        [HttpGet]
        public ActionResult Payment() {
            if (TempData["Ship"] == null)
            {
                return RedirectToAction("Shipping","Checkout");
            }
            var ship = (ShippingViewModel)TempData["Ship"];
            return View(ship);
        }

        [HttpPost]
        public ActionResult Payment(ShippingViewModel model, String PaymentMethod)
        {
            if(PaymentMethod.Equals("payment_cod")){
                return View("Checkout_Success", model);
            }

            return View();
        }
	}
}