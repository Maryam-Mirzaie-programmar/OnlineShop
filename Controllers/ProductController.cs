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


        [ChildActionOnly]
        public ActionResult ShowComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (! db.Products.Any(p => p.ProductID == id))
            {
                return HttpNotFound();
            }
            var comments = db.Product_Comments.Where(p => p.ProductID == id);
            return PartialView(comments);
        }

        public ActionResult CreateComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Products.Any(p => p.ProductID == id))
            {
                return HttpNotFound();
            }
            return PartialView(new Product_Comments() { ProductID = id.Value});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "CommentId, ProductID, Name, Email, Website, Comment, ParentId")] Product_Comments productComment)
        {
            if (ModelState.IsValid)
            {
                productComment.CreateDate = DateTime.Now;
                db.Product_Comments.Add(productComment);
                db.SaveChanges();
                return PartialView("ShowComments", db.Product_Comments.Where(p => p.ProductID == productComment.ProductID));
            }
            return PartialView(productComment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}