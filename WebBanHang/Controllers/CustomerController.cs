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
        public ActionResult Register(FormCollection form, Customer customer){
            int err = 0;
            var cusRepository = Repository.Create<CustomerRepository>();
            String userName = form["txtUser"];
            if (userName.Trim().Length == 0)
            {
                ViewData["ErrUser"] = "Vui long nhap username";
                err++;
            }
            else
            {
                if (cusRepository.checkExistCustomer(userName.Trim()))
                {
                    ViewData["ErrUser"] = "User ton tai";
                    err++;
                }
            }
            if(form["txtPassword"].Trim().Length == 0){
                ViewData["ErrPassword1"] = "Vui long nhap mat khau";
                err++;
            }
            else
            {
                if (!form["txtAgainPassword"].Trim().Equals(form["txtPassword"].Trim()))
                {
                    ViewData["ErrPassword2"] = "Hai mat khau khong khop";
                    err++;
                }
            }


            if (err == 0)
            {
                string s = "";
                return Content("Dang ky thanh cong!!!");
            }
            return View(); 
        }

        [HttpGet]
        public ActionResult Register()
        {
            int i = 1;
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
            if (ModelState.IsValid)
            {
                var customer = customerRepo.FindByEmail(model.Email);
                if (customer == null)
                {
                    ModelState.AddModelError("Email", "Email không tồn tại");
                    return View(model);
                }
                if (!EncryptUtils.PwdCompare(model.Password, customer.Passwrord))
                {
                    ModelState.AddModelError("Password", "Mật khẩu không chính xác");
                    return View(model);
                }
                FormsAuthentication.SetAuthCookie(model.Email,false);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }
	}
}