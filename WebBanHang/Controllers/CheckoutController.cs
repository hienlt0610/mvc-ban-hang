using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Models;
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
            var payments = Repository.Payment.FetchAll();
            ViewData["pCod"] = payments.SingleOrDefault(p=>p.PaymentType.Equals("cod"));
            ViewData["pAtm"] = payments.SingleOrDefault(p => p.PaymentType.Equals("atm"));
            ViewData["pOnline"] = payments.SingleOrDefault(p => p.PaymentType.Equals("online"));
            var ship = (ShippingViewModel)TempData["Ship"];
            return View(ship);
        }

        [HttpPost]
        public ActionResult Payment(ShippingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    CustomerID = model.CustomerID,
                    PaymentID = model.PaymentMethod,
                    OrderDate = DateTime.Now,
                    FullName = model.FullName,
                    Address = model.Address,
                    ProvinceID = model.ProvinceID,
                    DistrictID = model.DistrictID,
                    WardID = model.WardID,
                    Phone = model.Phone,
                    TotalPrice = ShoppingCart.Instance.GetTotal(),
                    Paid = false,
                    OrderStatusID = 1,
                    ShippingStatusID = 1,
                    Comment = model.Comment
                };
                var payment = Repository.Payment.FindById(model.PaymentMethod);

                //Checkout via COD
                if (payment.PaymentType.Equals("cod"))
                {
                    var newOrder = Repository.Order.Insert(order);
                    Repository.Order.SaveChanges();

                    if (newOrder != null && newOrder.OrderID != 0)
                    {
                        //Add each item from cart to orderdetail
                        var detailRepo = Repository.Create<OrderDetail>();
                        foreach(var cart in ShoppingCart.Instance.Items){
                            var od = new OrderDetail
                            {
                                OrderID = newOrder.OrderID,
                                ProductID = cart.ProductID,
                                Price = cart.Price,
                                Quantity = (byte)cart.Quantity,
                                ColorID = cart.ColorID,
                                Total = cart.TotalPrice
                            };
                            detailRepo.Insert(od);
                        }
                        Repository.SaveChanges();
                        ShoppingCart.Instance.Clean();
                        TempData["ship"] = newOrder;
                        return RedirectToAction("Success","Checkout");
                    }
                    ModelState.AddModelError("PaymentMethod", "Đã xảy ra lỗi, không thể đặt hàng!!!");
                }
                //Checkout via ATM
                else if (payment.PaymentType.Equals("atm"))
                {
                    TempData["ship"] = order;
                    return RedirectToAction("ATMCheckout", "Checkout");
                }
                //Checkout via Payment Online
                else if (payment.PaymentType.Equals("online"))
                {
                    TempData["ship"] = order;
                    return RedirectToAction("OnlineCheckout", "Checkout");
                }
            }
            model.Province = Repository.Province.FindById(model.ProvinceID);
            model.District = Repository.District.FindById(model.DistrictID);
            model.Ward = Repository.Ward.FindById(model.WardID);
            var payments = Repository.Payment.FetchAll();
            ViewData["pCod"] = payments.SingleOrDefault(p => p.PaymentType.Equals("cod"));
            ViewData["pAtm"] = payments.SingleOrDefault(p => p.PaymentType.Equals("atm"));
            ViewData["pOnline"] = payments.SingleOrDefault(p => p.PaymentType.Equals("online"));
            return View(model);
        }

        public ActionResult Success()
        {
            if (TempData["ship"]==null)
            {
                return RedirectToAction("Index","Home");
            }
            var model = (Order)TempData["ship"];
            return View("Checkout_Success", model);
        }
	}
}