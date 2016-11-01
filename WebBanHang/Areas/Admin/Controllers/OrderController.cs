using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Utils;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class OrderController : AdminBaseController
    {
        //
        // GET: /Admin/Order/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadOrder(int start, int length)
        {
            var orders = Repository.Order.FetchAll();
            var search = Request.QueryString["search[value]"].ToString();
            int orderIdSearch = 0;
            Int32.TryParse(search,out orderIdSearch);

            var ordersFilter = orders
                            .OrderByDescending(o => o.OrderDate)
                            .AsQueryable();
            if (!String.IsNullOrEmpty(search))
            {
                ordersFilter = ordersFilter.Where(o => o.OrderID == orderIdSearch);
            }
            if(ordersFilter.Count() > 0)
                ordersFilter = ordersFilter.Skip(start).Take(length);
            List<object> data = new List<object>();
            foreach (var order in ordersFilter)
            {
                List<object> row = new List<object>();
                row.Add(order.OrderID);

                //Trạng thái đơn đặt hàng
                var statusColor = "";
                switch (order.OrderStatusID)
                {
                    case 1:
                        statusColor = "warning";
                        break;
                    case 2:
                        statusColor = "info";
                        break;
                    case 3:
                        statusColor = "success";
                        break;
                    default:
                        statusColor = "danger";
                        break;
                }
                row.Add("<span data-pk='"+order.OrderID+"' data-value='"+order.OrderStatusID+"' class='label label-"+statusColor+" status-order'>"+order.OrderStatu.OrderStatusName+"</span>");
                
                //Trạng thái thanh toán
                string text;
                if (order.Paid)
                {
                    statusColor = "success";
                    text = "Đã thanh toán";
                }
                else
                {
                    statusColor = "warning";
                    text = "Chưa thanh toán";
                }
                row.Add("<span data-pk='" + order.OrderID + "' data-value='" + (order.Paid ? 1 : 0) + "' class='label label-" + statusColor + " status-payment'>" + text + "</span>");

                //Trạng thái giao hàng
                switch (order.ShippingStatusID)
                {
                    case 1:
                        statusColor = "danger";
                        break;
                    case 2:
                        statusColor = "info";
                        break;
                    case 3:
                        statusColor = "success";
                        break;
                }
                row.Add("<span data-pk='" + order.OrderID + "' data-value='" + order.ShippingStatusID + "' class='label label-" + statusColor + " status-shipping'>" + order.ShippingStatu.ShippingName + "</span>");

                //Thông tin khách hàng

                row.Add(order.Customer.FullName + "<br>("+order.Customer.Email+")");
                row.Add(order.Payment.PaymentName);
                row.Add(order.OrderDate.ToString("dd/M/yyyy hh:mm tt"));

                data.Add(row);
            }
            return Content(JsonConvert.SerializeObject(new {
                draw = Request.QueryString["draw"],
                recordsTotal = ordersFilter.Count(),
                recordsFiltered = ordersFilter.Count(),
                data = data
            }),"application/json");
        }

        public ActionResult LoadOrderProduct(int? id)
        {
            List<object> data = new List<object>();
            if (id == 0)
                return Content(JsonConvert.SerializeObject(new {
                    sum_total = 0,
                    data = data
                }),"application/json");

            var order = Repository.Order.FindById(id);
            if (order == null)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    sum_total = 0,
                    data = data
                }), "application/json");
            }

            foreach(var oProduct in order.OrderDetails){
                data.Add(new {
                    detail_id = oProduct.DetailID,
                    image_url = (oProduct.Product.ImageProducts.Count > 0) ? oProduct.Product.ImageProducts.FirstOrDefault().ImagePath : ImageHelper.DefaultImage(),
                    product_name = oProduct.Product.ProductName,
                    color = (oProduct.ColorID != null) ? "<span style=\"background-color: #"+oProduct.Color.HexCode+"\" class=\"label\">"+oProduct.Color.ColorName+"</span>" : "",
                    price = oProduct.Price,
                    quantity = oProduct.Quantity,
                    total = oProduct.Total
                });
            }

            return Content(JsonConvert.SerializeObject(new
            {
                data = data,
                sum_total = order.TotalPrice
            }), "application/json");
        }

        public ActionResult Detail(int? id)
        {
            if (id == null) return new HttpNotFoundResult("ID not found");
            var order = Repository.Order.FindById(id);
            if (order == null) return new HttpNotFoundResult("Order with id "+id+" does not exist in system");
            return View(order);
        }

        public ActionResult OrderStatusOption()
        {
            var orderStatus = Repository.Create<OrderStatu>().FetchAll();
            var listStatus = new List<object>();
            foreach (var status in orderStatus)
            {
                listStatus.Add(new
                {
                    value = status.OrderStatusID,
                    text = status.OrderStatusName
                });
            }
            return Content(JsonConvert.SerializeObject(listStatus), "application/json");
        }

        public ActionResult ShippingStatusOption()
        {
            var shippingStatus = Repository.Create<ShippingStatu>().FetchAll();
            var listStatus = new List<object>();
            foreach (var status in shippingStatus)
            {
                listStatus.Add(new
                {
                    value = status.ShippingStatusID,
                    text = status.ShippingName
                });
            }
            return Content(JsonConvert.SerializeObject(listStatus), "application/json");
        }

        [HttpPost]
        public ActionResult UpdateStatus(int? id, [Bind(Prefix = "order_status")] int? orderStatus = null, [Bind(Prefix = "shipping_status")] int? shippingStatus = null)
        {
            dynamic result = new ExpandoObject();
            result.status = false;
            result.message = "";
            if(id == null){
                result.message = "Thiếu mã đơn đặt hàng";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }
            if(orderStatus == null && shippingStatus == null){
                result.message = "Phải có ít nhất 1 trong 2 tham số orderStatus và shippingStatus";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var order = Repository.Order.FindById(id);
            if(order == null){
                result.message = "Đơn đặt hàng này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            int oldOderStatus = order.OrderStatusID;
            int oldShipping = order.ShippingStatusID;

            if (orderStatus != null)
            {
                var repo = Repository.Create<OrderStatu>();
                if (!repo.FetchAll().Any(s => s.OrderStatusID == orderStatus))
                {
                    result.message = "Mã orderStatus không hợp lệ";
                    return Content(JsonConvert.SerializeObject(result), "application/json");
                }
                order.OrderStatusID = orderStatus.Value;
            }

            if (shippingStatus != null)
            {
                var repo = Repository.Create<ShippingStatu>();
                if (!repo.FetchAll().Any(s => s.ShippingStatusID == shippingStatus))
                {
                    result.message = "Mã shippingStatus không hợp lệ";
                    return Content(JsonConvert.SerializeObject(result), "application/json");
                }
                order.ShippingStatusID = shippingStatus.Value;
            }
            Repository.Order.SaveChanges();
            result.status = true;
            result.message = "Update thành công";
            result.order_class_remove = getClassOrderStatus(oldOderStatus);
            result.order_class_add = getClassOrderStatus(order.OrderStatusID);
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult UpdatePayment(int? id, bool paid = false)
        {
            dynamic result = new ExpandoObject();
            result.success = false;
            result.message = "";
            if (id == null)
            {
                result.message = "Thiếu mã đơn đặt hàng";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }
            var order = Repository.Order.FindById(id);
            if (order == null)
            {
                result.message = "Đơn đặt hàng này không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            order.Paid = paid;
            Repository.Order.SaveChanges();
            result.success = true;
            result.message = "Cập nhật thành công";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult RenoveProductOrder(OrderDetail orderDetail)
        {
            dynamic result = new ExpandoObject();
            result.status = "error";
            result.title = "Loại bỏ thất bại";
            result.message = "";
            if (orderDetail.DetailID == 0 || orderDetail.OrderID == 0)
            {
                result.message = "Thiếu thông số";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }
            var order = Repository.Order.FindById(orderDetail.OrderID);
            if (order == null)
            {
                result.message = "Đơn đặt hàng không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            var oldDetail = order.OrderDetails.FirstOrDefault(o=>o.DetailID == orderDetail.DetailID);
            if(oldDetail == null){
                result.message = "Sản phẩm này không tồn tại trong đơn đặt hàng";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            order.OrderDetails.Remove(oldDetail);
            var sum = order.OrderDetails.Sum(o=>o.Total);
            order.TotalPrice = sum;
            Repository.Order.SaveChanges();

            result.status = "success";
            result.title = "Loại bỏ thành công";
            result.message = "Chúc mừng bạn đã loại bỏ sản phẩm khỏi đơn đặt hàng thành công";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult Delete(int? id)
        {
            if(id == null){
                return HttpNotFound();
            }
            var order = Repository.Order.FindById(id);
            if(order == null)
                return RedirectToAction("Index", "Order");
            Repository.Order.Delete(id);
            Repository.SaveChanges();
            return RedirectToAction("Index","Order");
        }

        private String getClassOrderStatus(int status)
        {
            var statusColor = "";
            switch (status)
            {
                case 1:
                    statusColor = "warning";
                    break;
                case 2:
                    statusColor = "info";
                    break;
                case 3:
                    statusColor = "success";
                    break;
                default:
                    statusColor = "danger";
                    break;
            }
            return "label-" + statusColor;
        }
	}
}