using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;
using WebBanHang.Core.RepositoryModel;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
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
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form, Customer customer)
        {
            return View();
        }
	}
}