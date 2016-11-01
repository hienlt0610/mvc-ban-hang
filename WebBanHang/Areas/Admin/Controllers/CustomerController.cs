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
    public class CustomerController : AdminBaseController
    {
        //
        // GET: /Admin/Customer/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load(int start, int length)
        {
            var search = Request["search[value]"];
            var customers = Repository.Customer.FetchAll().OrderByDescending(c=>c.RegistrationDate).AsQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                customers = customers.Where(c => 
                    c.FullName.ToLower().Contains(search)
                    || c.Email.ToLower().Contains(search)
                );
            }

            int record_count = customers.Count();
            customers = customers.Skip(start).Take(length);
            List<object> data = new List<object>();
            foreach(var customer in customers){
                var latest_order = customer.Orders.OrderByDescending(o=>o.OrderDate).FirstOrDefault();
                data.Add(new { 
                    customer_id = customer.CustomerID,
                    customer_name = customer.FullName,
                    customer_email = customer.Email,
                    order_num = customer.Orders.Count,
                    order_latest = (latest_order != null) ? latest_order.OrderID : 0,
                    total_pay = customer.Orders.Where(o=>o.Paid).Sum(o=>o.TotalPrice)
                });
            }

            return Content(JsonConvert.SerializeObject(new { 
                draw = Request["draw"],
                data = data,
                recordsFiltered = record_count,
                recordsTotal = record_count
            }),"application/json");
        }

        public ActionResult Detail(int? id)
        {
            if(id == null){
                return HttpNotFound();
            }
            var customer = Repository.Customer.FindById(id);
            if(customer == null){
                return HttpNotFound();
            }
            ViewBag.Provinces = Repository.Province.FetchAll().OrderBy(p=>p.Type);
            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            dynamic result = new ExpandoObject();
            result.success = false;
            result.message = "";
            if(customer.CustomerID == 0){
                result.message = "Chưa có mã khách hàng";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }

            var oldCustomer = Repository.Customer.FindById(customer.CustomerID);
            if (oldCustomer == null)
            {
                result.message = "Khách hàng không tồn tại";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            oldCustomer.FullName = customer.FullName;
            oldCustomer.Phone = customer.Phone;
            oldCustomer.Address = customer.Address;
            oldCustomer.ProvinceID = customer.ProvinceID;
            oldCustomer.DistrictID = customer.DistrictID;
            oldCustomer.WardID = customer.WardID;
            Repository.Customer.SaveChanges();
            result.success = true;
            result.message = "Cập nhật thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            dynamic result = new ExpandoObject();
            result.success = false;
            result.message = "";
            if(id == null){
                result.message = "Thiếu mã khách hàng";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }

            var customer = Repository.Customer.FindById(id);
            if(customer == null){
                result.message = "Khách hàng này không tồn tại trong hệ thống";
                return Content(JsonConvert.SerializeObject(result), "application/json");
            }
            Repository.Customer.Delete(id);
            Repository.Customer.SaveChanges();
            result.success = true;
            result.message = "Xóa khách hàng khỏi hệ thống thành công";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
	}
}