using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Areas.UserPanel.Controllers
{
    public class HomeController : Controller
    {
        // GET: UserPanel/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}