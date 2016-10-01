using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHang.Core;
using WebBanHang.Utils;
using WebBanHang.ViewModels;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class AuthController : AdminBaseController
    {
        //
        // GET: /Admin/Auth/
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }

        [OnlyGuest]
        public ActionResult Login()
        {
            return View(new AdminLoginViewModel());
        }

        [HttpPost]
        [OnlyGuest]
        public ActionResult Login(AdminLoginViewModel model)
        {
            var user = Repository.User.FindByUserName(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Tài khoản không chính xác");
            }
            if (user != null && !EncryptUtils.PwdCompare(model.Password, user.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không chính xác");
            }

            if(ModelState.IsValid)
            {
                Response.SetAuthCookie(FormsAuthentication.FormsCookieName+"_ADMIN",model.Remember,user.UserID);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        public ActionResult Logout(){
            if (Request.Cookies[FormsAuthentication.FormsCookieName+"_ADMIN"] != null)
            {
                HttpCookie myCookie = new HttpCookie(FormsAuthentication.FormsCookieName + "_ADMIN");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin"});
        }
	}
}