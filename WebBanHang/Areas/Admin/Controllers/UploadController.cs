using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Security]
    public class UploadController : AdminBaseController
    {
        //
        // GET: /Admin/Upload/
        public ActionResult Index()
        {
            return View();
        }
	}
}