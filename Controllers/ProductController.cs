using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datalayer;
using System.Net;

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

        public ActionResult LastProducts()
        {
            var products = db.Products.OrderByDescending(p => p.ProductCreateDate).Take(6);
            return PartialView(products);
        }

        public ActionResult MostSellingProducts()
        {
            var products = db.Products.OrderByDescending(p => p.ProductCreateDate).Take(6);
            return PartialView(products);
        }

        [Route("SingleProduct/{id}")]
        public ActionResult SingleProduct(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if(product == null)
            {
                return HttpNotFound();  
            }
            var featureIds = product.ProductFeatures.GroupBy(f => f.FeatureId).Select(f => f.Key).ToList();
            List<ProductFeatureViewModel> features = new List<ProductFeatureViewModel>();
            foreach(var featureId in featureIds)
            {
                var productFeature = new ProductFeatureViewModel()
                {
                    FeatureTitle = db.Features.Single(f => f.FeatureId == featureId).FeatureTitle,
                    Values = product.ProductFeatures.Where(f => f.FeatureId == featureId).Select(f => f.Value).ToList()
                };

                features.Add(productFeature);
            }
            ViewBag.ProductFeatures = features;
            return View(product);
        }
    }
}