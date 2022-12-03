using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datalayer;

namespace MyEshop.Controllers
{
    public class HomeController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Slider()
        {
            DateTime now = DateTime.Now.Date;
            var slides = db.Sliders.Where(s => s.StartDate <= now && s.EndDate>= now && s.IsActive == true).ToList();
            return PartialView(slides);
        }
    }
}