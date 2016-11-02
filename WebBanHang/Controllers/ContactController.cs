using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Models;
using WebBanHang.ViewModels;

namespace WebBanHang.Controllers
{
    public class ContactController : BaseController
    {
        //
        // GET: /Contact/
        public ActionResult Index()
        {
            var model = new ContactViewModel();
            if(Request.IsAuthenticated){
                model.FullName = UserManager.CurrentCustomer.FullName;
                model.Email = UserManager.CurrentCustomer.Email;
                model.Phone = UserManager.CurrentCustomer.Phone;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SendContact(ContactViewModel model)
        {
            dynamic result = new ExpandoObject();
            result.success = false;
            result.message = "";
            if (String.IsNullOrEmpty(model.FullName) || String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Phone) || String.IsNullOrEmpty(model.Message))
            {
                result.message = "Rất tiêc! Đã có vấn đề gì đó xảy ra, nên không thể gửi liên hệ được, vui lòng thử lại";
                return Content(JsonConvert.SerializeObject(result),"application/json");
            }
            var repo = Repository.Create<Contact>();
            var ip = Request.UserHostAddress;
            var oldContact = repo.FetchAll().OrderByDescending(c=>c.Time).FirstOrDefault(c=>c.IP.Equals(ip));
            if(oldContact != null){
                var timeDelay = Math.Abs((DateTime.Now - oldContact.Time).TotalSeconds);
                if(timeDelay <= 60){
                    result.message = "Phát hiện gửi nhiều lần, vui lòng không được gửi nhiều lần cùng lúc";
                    return Content(JsonConvert.SerializeObject(result), "application/json");
                }
            }
            var contact = new Contact()
            {
                FullName = model.FullName,
                Email = model.Email,
                Phone = model.Phone,
                Message= model.Message,
                IP = ip,
                Time = DateTime.Now
            };
            if (Request.IsAuthenticated) contact.CustomerID = UserManager.CurrentCustomer.CustomerID;
            
            repo.Insert(contact);
            repo.SaveChanges();
            result.success = true;
            result.message = "Cảm ơn, liên hệ của bạn đã được gửi thành công!!!";
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
	}
}