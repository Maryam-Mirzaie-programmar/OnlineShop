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
    public class SlidersController : Controller
    {
        private OnlineShopDBEntities db = new OnlineShopDBEntities();

        // GET: Admin/Sliders
        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

        // GET: Admin/Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SliderId,Title,ImageName,StartDate,EndDate,IsActive,Url")] Slider slider , HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if(imgUp == null || !imgUp.IsImage())
                {
                    ModelState.AddModelError("ImageName", "لطفا یک تصویر انتخاب کنید");
                    return View(slider);
                }

                slider.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                imgUp.SaveAs(Server.MapPath("/Images/SliderImages/" + slider.ImageName));
                ImageResizer img = new ImageResizer();
                img.Resize(Server.MapPath("/Images/SliderImages/" + slider.ImageName), Server.MapPath("/Images/SliderImages/Thumb/" + slider.ImageName));

                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SliderId,Title,ImageName,StartDate,EndDate,IsActive,Url")] Slider slider, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    System.IO.File.Delete(Server.MapPath("/Images/SliderImages/" + slider.ImageName));
                    System.IO.File.Delete(Server.MapPath("/Images/SliderImages/Thumb/" + slider.ImageName));

                    slider.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/SliderImages/" + slider.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/SliderImages/" + slider.ImageName), Server.MapPath("/Images/SliderImages/Thumb/" + slider.ImageName));
                }
                    db.Entry(slider).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            System.IO.File.Delete(Server.MapPath("/Images/SliderImages/" + slider.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/SliderImages/Thumb/" + slider.ImageName));
            db.Sliders.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
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
