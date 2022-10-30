using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Datalayer;

namespace MyEshop.Areas.Admin.Controllers
{
    public class Product_GroupsController : Controller
    {
        private OnlineShopDBEntities db = new OnlineShopDBEntities();

        // GET: Admin/Product_Groups
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListGroups()
        {
            var product_Groups = db.Product_Groups.Include(p => p.Product_Groups2);
            return PartialView(product_Groups.ToList());
        }

        // GET: Admin/Product_Groups/Create
        public ActionResult Create(int? parentId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach(var item in db.Product_Groups.Where(g => g.ParentId == null))
            {

                items.Add(new SelectListItem { Text = item.GroupTitle, Value = item.GroupId.ToString(), Selected = (item.GroupId == parentId) });
            }
            ViewBag.ParentId = items ;
            return PartialView();
        }

        // POST: Admin/Product_Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupId,GroupTitle,ParentId")] Product_Groups product_Groups)
        {
            if (ModelState.IsValid)
            {
                db.Product_Groups.Add(product_Groups);
                db.SaveChanges();
                return PartialView("ListGroups", db.Product_Groups);
            }

            ViewBag.ParentId = new SelectList(db.Product_Groups, "GroupId", "GroupTitle", product_Groups.ParentId);
            return PartialView(product_Groups);
        }

        // GET: Admin/Product_Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            if (product_Groups == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Product_Groups.Where(g => g.ParentId == null), "GroupId", "GroupTitle", product_Groups.ParentId);
            return PartialView(product_Groups);
        }

        // POST: Admin/Product_Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupId,GroupTitle,ParentId")] Product_Groups product_Groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Groups).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("ListGroups", db.Product_Groups);
            }
            ViewBag.ParentId = new SelectList(db.Product_Groups.Where(g => g.ParentId == null), "GroupId", "GroupTitle", product_Groups.ParentId);
            return PartialView(product_Groups);
        }

        // GET: Admin/Product_Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            if (product_Groups == null)
            {
                return HttpNotFound();
            }
            return PartialView(product_Groups);
        }

        // POST: Admin/Product_Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Groups product_Groups = db.Product_Groups.Find(id);
            if(product_Groups.Product_Groups1.Any())
            {
                foreach (var subgroup in db.Product_Groups.Where(g => g.ParentId == product_Groups.GroupId))
                {
                    db.Product_Groups.Remove(subgroup);
                }
            }
            db.Product_Groups.Remove(product_Groups);
            db.SaveChanges();
            return PartialView("ListGroups", db.Product_Groups);
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
