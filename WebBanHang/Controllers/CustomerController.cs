using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Principal;
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
        //
        // GET: /User/
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        [OnlyGuest]
        public ActionResult Register(SignUpViewModel model){
            var existCustomer = Repository.Customer.FindByEmail(model.Email);
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
                    RegistrationDate = DateTime.Now
                };
                customer = Repository.Customer.Insert(customer);
                Repository.Customer.SaveChanges();
                SyncLogin(customer,false);
                return RedirectToAction("Index", "Home");
            }
            return View(model); 
        }

        [HttpGet]
        [OnlyGuest]
        public ActionResult Register()
        {
            return View();   
        }

        [HttpGet]
        [OnlyGuest]
        public ActionResult Login()
        {
            return View(new SignInViewModel());
        }

        [HttpPost]
        [OnlyGuest]
        public ActionResult Login(SignInViewModel model)
        {
            var customer = Repository.Customer.FindByEmail(model.Email);
            if (customer == null)
            {
                ModelState.AddModelError("Email", "Email không tồn tại");
            }
            if (customer != null && !EncryptUtils.PwdCompare(model.Password, customer.Passwrord))
            {
                ModelState.AddModelError("Password", "Mật khẩu không chính xác");
            }
            if (ModelState.IsValid)
            {
                SyncLogin(customer,model.Remember);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public ActionResult Test()
        {
            var session = Session["b"];
            return Content("Hello");
        }

        public ActionResult FacebookLogin()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var access_token = result.access_token;
            if (!String.IsNullOrEmpty(access_token))
            {
                fb.AccessToken = access_token;
                dynamic me = fb.Get("me?fields=id,email,name");
                String fbID = me.id;
                fbID = fbID.Trim();
                if (!string.IsNullOrEmpty(fbID))
                {
                    Customer customer = Repository.Customer.FindByFacebookID(fbID);
                    if (customer == null)
                    {
                        customer = new Customer
                        {
                            FacebookID = me.id,
                            Email = me.email,
                            FullName = me.name,
                            Status = true,
                            RegistrationDate = DateTime.Now
                        };
                        customer = Repository.Customer.Insert(customer);
                        Repository.Customer.SaveChanges();
                    }
                    SyncLogin(customer, false);
                }
            }
            return RedirectToAction("Index","Home");
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        private void SyncLogin(Customer userdata, bool remember)
        {
            if (userdata == null) return;
            Response.SetAuthCookie(FormsAuthentication.FormsCookieName, remember, userdata.CustomerID);
        }
	}
}