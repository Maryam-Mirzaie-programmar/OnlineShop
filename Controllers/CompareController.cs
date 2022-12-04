using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datalayer;

namespace MyEshop.Controllers
{
    public class CompareController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();

        // GET: Compare
        public ActionResult Index()
        {
            List<CompareItemViewModel> compareItems = new List<CompareItemViewModel>();

            if (Session["Compare"] != null)
            {
                compareItems = Session["Compare"] as List<CompareItemViewModel>;
            }

            List<Feature> features = new List<Feature>();
            List<ProductFeature> productfeatures = new List<ProductFeature>();

            foreach (var item in compareItems)
            {           
                features.AddRange(db.ProductFeatures.Where(p => p.ProductID == item.ProductID).Select(p => p.Feature).ToList());
                productfeatures.AddRange(db.ProductFeatures.Where(p => p.ProductID == item.ProductID).ToList());
            }

            ViewBag.features = features.Distinct().ToList();
            ViewBag.productfeatures = productfeatures;


            return View(compareItems);
        }

        public ActionResult CompareAside()
        {
            List<CompareItemViewModel> compareList = new List<CompareItemViewModel>();
            if (Session["Compare"] != null)
            {
                compareList = Session["Compare"] as List<CompareItemViewModel>;
            }
            return PartialView(compareList);
        }

        public ActionResult AddToCompare(int id)
        {
            List<CompareItemViewModel> compareList = new List<CompareItemViewModel>();
            if (Session["Compare"] != null)
            {
                compareList = Session["Compare"] as List<CompareItemViewModel>;
            }
            if(!compareList.Any(c=> c.ProductID == id)  && db.Products.Any(p=> p.ProductID == id))
            {
                var product = db.Products.Where(p => p.ProductID == id).Select(p => new { p.ProductTitle, p.ProductImageName }).Single();
                compareList.Add(new CompareItemViewModel()
                {
                    ProductID = id,
                    Title = product.ProductTitle,
                    ImageName = product.ProductImageName
                });
            }
            Session["Compare"] = compareList;
            return PartialView("CompareAside", compareList);
        }

        public ActionResult RemoveFromCompare(int id)
        {
            List<CompareItemViewModel> compareList = new List<CompareItemViewModel>();
            if (Session["Compare"] != null)
            {
                compareList = Session["Compare"] as List<CompareItemViewModel>;
            }
            if (compareList.Any(c => c.ProductID == id))
            {
                int index = compareList.FindIndex(c => c.ProductID == id);
                compareList.RemoveAt(index);
            }
            Session["Compare"] = compareList;
            return PartialView("CompareAside", compareList);
        }
    }
}