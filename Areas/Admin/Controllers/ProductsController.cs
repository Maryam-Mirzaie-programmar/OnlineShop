using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datalayer;
using System.IO;

namespace MyEshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private OnlineShopDBEntities db = new OnlineShopDBEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductGroups = db.Product_Groups.ToList(); 
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductTitle,ProductShortDescription,ProductText,ProductPrice,ProductImageName")] Product product , List<int> selectedGroups , HttpPostedFileBase imgUp, string tags)
        {
            if (ModelState.IsValid)
            {
                if(selectedGroups == null)
                {
                    ViewBag.SelectedGroupsError = true;

                    ViewBag.ProductGroups = db.Product_Groups.ToList();
                    return View(product);
                }
                foreach(var groupId in selectedGroups)
                {
                    db.Product_SelectedGroups.Add(new Product_SelectedGroups()
                    {
                        ProductID = product.ProductID,
                        GroupId = groupId
                    });
                }
                product.ProductImageName = "default_image.png";
                if (imgUp != null && imgUp.IsImage())
                {
                    product.ProductImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/ProductImages/" + product.ProductImageName));
                    ImageResizer imageResizer = new ImageResizer();
                    imageResizer.Resize(Server.MapPath("/Images/ProductImages/" + product.ProductImageName), Server.MapPath("/Images/ProductImages/Thumb/" + product.ProductImageName));
                }

                if(!String.IsNullOrWhiteSpace(tags))
                {
                    foreach(var tag in tags.Split('،'))
                    {
                        if (!String.IsNullOrWhiteSpace(tag))
                        {
                            db.Product_Tags.Add(new Product_Tags()
                            {
                                ProductID = product.ProductID,
                                ProductTag = tag.Trim()
                            });
                        }
                    }
                }

                product.ProductCreateDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductGroups = db.Product_Groups.ToList();
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

          
            ViewBag.Tags = String.Join("، ", product.Product_Tags.Select(t => t.ProductTag));
            ViewBag.ProductSelectedGrops = product.Product_SelectedGroups.ToList();
            ViewBag.ProductGroups = db.Product_Groups.ToList();
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductTitle,ProductShortDescription,ProductText,ProductPrice,ProductImageName,ProductCreateDate")] Product product, List<int> selectedGroups, HttpPostedFileBase imgUp, string tags)
        {
            if (ModelState.IsValid)
            {
                if (selectedGroups == null)
                {
                    ViewBag.SelectedGroupsError = true;

                    ViewBag.Tags = tags;
                    ViewBag.ProductSelectedGrops = selectedGroups;
                    ViewBag.ProductGroups = db.Product_Groups.ToList();
                    return View(product);
                }


                db.Product_SelectedGroups.Where(g => g.ProductID == product.ProductID).ToList().ForEach(g => db.Product_SelectedGroups.Remove(g));
                foreach (var groupId in selectedGroups)
                {
                    db.Product_SelectedGroups.Add(new Product_SelectedGroups()
                    {
                        ProductID = product.ProductID,
                        GroupId = groupId
                    });
                }

                if (imgUp != null && imgUp.IsImage())
                {
                    if(product.ProductImageName != "default_image.png")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + product.ProductImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + product.ProductImageName));
                    }
                    product.ProductImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/ProductImages/" + product.ProductImageName));
                    ImageResizer imageResizer = new ImageResizer();
                    imageResizer.Resize(Server.MapPath("/Images/ProductImages/" + product.ProductImageName), Server.MapPath("/Images/ProductImages/Thumb/" + product.ProductImageName));
                }

                db.Product_Tags.Where(t => t.ProductID == product.ProductID).ToList().ForEach(t => db.Product_Tags.Remove(t));
                if (!String.IsNullOrWhiteSpace(tags))
                {
                    foreach (var tag in tags.Split('،'))
                    {
                        if (!String.IsNullOrWhiteSpace(tag))
                        {
                            db.Product_Tags.Add(new Product_Tags()
                            {
                                ProductID = product.ProductID,
                                ProductTag = tag.Trim()
                            });
                        }
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.Tags = tags;
            ViewBag.ProductSelectedGrops = selectedGroups;
            ViewBag.ProductGroups = db.Product_Groups.ToList();
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Gallery(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(! db.Products.Any(p => p.ProductID == id))
            {
                return HttpNotFound();
            }

            var gallery = new Product_Gallery() { ProductID = id};
            ViewBag.Galleris = db.Product_Gallery.Where(g => g.ProductID == id).ToList();
            return View(gallery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gallery(Product_Gallery gallery, HttpPostedFileBase imgUp)
        {
            if(ModelState.IsValid)
            {
                if(imgUp != null && imgUp.IsImage())
                {
                    gallery.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/ProductImages/" + gallery.ImageName));
                    ImageResizer imageResizer = new ImageResizer();
                    imageResizer.Resize(Server.MapPath("/Images/ProductImages/" + gallery.ImageName), Server.MapPath("/Images/ProductImages/Thumb/" + gallery.ImageName));

                    db.Product_Gallery.Add(gallery);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Gallery" , new {id = gallery.ProductID});
        }

        public ActionResult DeleteGallery(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gallery = db.Product_Gallery.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }

            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + gallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + gallery.ImageName));

            db.Product_Gallery.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        public ActionResult ProductFeatures(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!db.Products.Any(p => p.ProductID == id))
            {
                return HttpNotFound();
            }
            var productFeature = new ProductFeature()
            {
                ProductID = id
            };


            ViewBag.FeatureId = new SelectList(db.Features, "FeatureId", "FeatureTitle");
            ViewBag.Features = db.ProductFeatures.Where(p => p.ProductID == id).ToList();
            return View(productFeature);
        }

        [HttpPost]
        public ActionResult ProductFeatures(ProductFeature feature)
        {
            if(ModelState.IsValid)
            {
                db.ProductFeatures.Add(feature);
                db.SaveChanges();
            }
            return RedirectToAction("ProductFeatures", new {id = feature.ProductID});
        }

        public ActionResult DeleteProductFeature(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductFeature productFeature = db.ProductFeatures.Find(id);
            if (productFeature == null)
            {
                return HttpNotFound();
            }

            db.ProductFeatures.Remove(productFeature);
            db.SaveChanges();
            return RedirectToAction("ProductFeatures", new { id = productFeature.ProductID });
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
