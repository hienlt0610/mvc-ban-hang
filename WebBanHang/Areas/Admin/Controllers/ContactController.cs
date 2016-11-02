using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Utils;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class ContactController : AdminBaseController
    {
        //
        // GET: /Admin/Contact/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load(int start, int length)
        {
            var repo = Repository.Create<Contact>();
            List<object> data = new List<object>();
            var contacts = repo.FetchAll().OrderByDescending(c=>c.Time).AsQueryable();
            var recordCount = contacts.Count();
            contacts = contacts.Skip(start).Take(length);
            foreach (var contact in contacts)
            {
                data.Add(new
                {
                    contact_id = contact.ContactID,
                    full_name = contact.FullName,
                    email = contact.Email,
                    phone = contact.Phone,
                    message = contact.Message.Truncate(90,true,true),
                    customer_id = contact.CustomerID,
                    ip = contact.IP,
                    time = contact.Time.ToRelativeString(),
                    seen = contact.Seen
                });
            }
            return Content(JsonConvert.SerializeObject(new
            {
                draw = Request["draw"],
                data = data,
                recordsFiltered = recordCount,
                recordsTotal = recordCount
            }), "application/json");
        }

        public ActionResult Detail(int? id)
        {
            if (id == null) return HttpNotFound();
            var repo = Repository.Create<Contact>();
            var contact = repo.FindById(id);
            if (contact == null) return HttpNotFound();
            if (!contact.Seen)
            {
                contact.Seen = true;
                repo.SaveChanges();
            }
            return View(contact);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return HttpNotFound();
            var repo = Repository.Create<Contact>();
            var contact = repo.FindById(id);
            if (contact == null) return HttpNotFound();
            repo.Delete(id);
            repo.SaveChanges();
            return RedirectToAction("Index","Contact");
        }
	}
}