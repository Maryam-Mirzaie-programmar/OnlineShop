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

        [Route("Archieve")]
        public ActionResult Archieve(int pageId=1, string title="", int minPrice=0, int maxPrice=0 , List<int> selectedGroups = null)
        {
            List<Product> products = new List<Product>();
            if(selectedGroups != null)
            {
               foreach(var groupId in selectedGroups)
                {
                    products.AddRange(db.Product_SelectedGroups.Where(g => g.GroupId == groupId).Select(g=> g.Product).ToList());
                }
               products = products.Distinct().ToList();
            }
            else
            {
                products = db.Products.ToList();
            }
            if(! String.IsNullOrWhiteSpace(title))
            {
                title = title.Trim().ToLower();
                products = products.Where(p => p.ProductTitle.ToLower().Contains(title)).ToList();
            }
            if(minPrice > 0)
            {
                products = products.Where(p => p.ProductPrice >= minPrice).ToList();
            }
            if (maxPrice > 0)
            {
                products = products.Where(p => p.ProductPrice <= maxPrice).ToList();
            }
            ViewBag.selectedGroupsList = selectedGroups;
            ViewBag.PageId = pageId;
            ViewBag.groups = db.Product_Groups.ToList();

            //Paging
            int take = 3;
            int skip = (pageId - 1) * take;
            // ViewBag.PagesCount = (int) Math.Ceiling(products.Count / take);
            double pegeCount = 1.0 * products.Count / take;
            ViewBag.PagesCount = Math.Ceiling(pegeCount);
            return View(products.OrderByDescending(p => p.ProductCreateDate).Skip(skip).Take(take));
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