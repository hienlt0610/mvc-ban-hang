using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHang.Core;
using WebBanHang.Core.RepositoryModel;
using WebBanHang.Models;
using WebBanHang.Utils;
using WebBanHang.ViewModels;

namespace WebBanHang.Controllers
{
    public class CustomerController : BaseController
    {
        CustomerRepository customerRepo;
        //
        // GET: /User/
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }

        public CustomerController()
        {
            customerRepo = Repository.Create<CustomerRepository>();
        }

        [HttpPost]
        public ActionResult Register(SignUpViewModel model){
            var existCustomer = customerRepo.FindByEmail(model.Email);
            if (existCustomer != null)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại...");
            }
            if (ModelState.IsValid)
            {
                Customer customer = new Customer { 
                    Email = model.Email,
                    Passwrord=EncryptUtils.MD5(model.Password),
                    FullName = model.FullName,
                    Status = false,
                    RegistrationDate =DateTime.Now
                };
                customerRepo.Insert(customer);
                customerRepo.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model); 
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();   
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new SignInViewModel());
        }

        [HttpPost]
        public ActionResult Login(SignInViewModel model)
        {
            var customer = customerRepo.FindByEmail(model.Email);
            if (customer == null)
            {
                ModelState.AddModelError("Email", "Email không tồn tại");
            }
            if (!EncryptUtils.PwdCompare(model.Password, customer.Passwrord))
            {
                ModelState.AddModelError("Password", "Mật khẩu không chính xác");
            }
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.Email,false);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
	}
}