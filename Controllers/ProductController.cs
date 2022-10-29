using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datalayer;

namespace MyEshop.Controllers
{
    public class ProductController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();


        [ChildActionOnly]
        public ActionResult ShowProductGroups()
        {
            return PartialView(db.Product_Groups);   
        }
    }
}