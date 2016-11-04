using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    return RedirectToAction("OnePayNoiDia", "Checkout");
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

        public ActionResult OnePayNoiDia()
        {
            string amount = (ShoppingCart.Instance.GetTotal() * 100).ToString();
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayProperty.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayProperty.HASH_CODE);

            conn.AddDigitalOrderField("Title", "Thanh toán trực tuyến");
            conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", OnepayProperty.VERSION);
            conn.AddDigitalOrderField("vpc_Command", OnepayProperty.COMMAND);
            conn.AddDigitalOrderField("vpc_Merchant", OnepayProperty.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", OnepayProperty.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", RandomString());
            conn.AddDigitalOrderField("vpc_OrderInfo", "Test đơn hàng");
            conn.AddDigitalOrderField("vpc_Amount", amount);
            conn.AddDigitalOrderField("vpc_Currency", "VND");
            conn.AddDigitalOrderField("vpc_ReturnURL", Url.Action("OnePayNoiDiaRes", "CheckOut", null, Request.Url.Scheme, null));

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            conn.AddDigitalOrderField("vpc_Customer_Phone", "");
            conn.AddDigitalOrderField("vpc_Customer_Email", "");
            conn.AddDigitalOrderField("vpc_Customer_Id", "");
            conn.AddDigitalOrderField("vpc_TicketNo", Request.UserHostAddress);

            string url = conn.Create3PartyQueryString();
            return Redirect(url);
        }

        public ActionResult OnePayNoiDiaRes()
        {
            string hashvalidateResult = "";

            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnepayProperty.URL_ONEPAY_TEST);
            conn.SetSecureSecret(OnepayProperty.HASH_CODE);

            // Xu ly tham so tra ve va du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);

            // Lay tham so tra ve tu cong thanh toan
            string vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode");
            string amount = conn.GetResultField("vpc_Amount");
            string localed = conn.GetResultField("vpc_Locale");
            string command = conn.GetResultField("vpc_Command");
            string version = conn.GetResultField("vpc_Version");
            string cardType = conn.GetResultField("vpc_Card");
            string orderInfo = conn.GetResultField("vpc_OrderInfo");
            string merchantID = conn.GetResultField("vpc_Merchant");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef");
            string transactionNo = conn.GetResultField("vpc_TransactionNo");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message");

            // Kiem tra 2 tham so tra ve quan trong nhat
            if (hashvalidateResult.Equals("CORRECTED") && txnResponseCode.Trim() == "0")
            {
                return Content("PaySuccess");
            }
            else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
            {
                return Content("PayPending");
            }
            else
            {
                return Content("PayUnSuccess");
            }
        }

        private string RandomString()
        {
            var str = new StringBuilder();
            var random = new Random();
            for (int i = 0; i <= 5; i++)
            {
                var c = Convert.ToChar(Convert.ToInt32(random.Next(65, 68)));
                str.Append(c);
            }
            return str.ToString().ToLower();
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