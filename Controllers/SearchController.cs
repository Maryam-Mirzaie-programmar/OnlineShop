using Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class SearchController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();

        // GET: Search
        public ActionResult Index(string q)
        {
            var products = new List<Product>();
            if(! String.IsNullOrWhiteSpace(q))
            {
                q = q.ToLower().Trim();
                products.AddRange(db.Products.Where(p => p.ProductTitle.ToLower().Contains(q)));
                products.AddRange(db.Products.Where(p => p.ProductShortDescription.ToLower().Contains(q)));
                products.AddRange(db.Product_Tags.Where(t => t.ProductTag == q).Select(t => t.Product));
            }
            return View(products.Distinct());
        }
    }
}