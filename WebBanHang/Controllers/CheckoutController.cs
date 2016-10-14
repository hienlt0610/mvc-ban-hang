using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;

namespace WebBanHang.Controllers
{
    [Authorize]
    public class CheckoutController : BaseController
    {
        //
        // GET: /Checkout/
        public ActionResult Index()
        {
            return View();
        }
	}
}