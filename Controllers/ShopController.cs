using Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class ShopController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Orders()
        {
            List<OrderFactorViewModel> ordersList = new List<OrderFactorViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> itemsList = Session["ShopCart"] as List<ShopCartItem>;
                foreach (var item in itemsList)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new { p.ProductImageName, p.ProductTitle ,  p.ProductPrice }).Single();
                    ordersList.Add(new OrderFactorViewModel()
                    {
                        ProductID = item.ProductID,
                        Count = item.Count,
                        Title = product.ProductTitle,
                        ImageName = product.ProductImageName,
                        Price = product.ProductPrice,
                        Sum = item.Count * product.ProductPrice

                    });
                }
            }
            return PartialView(ordersList);
        }

        public ActionResult ChangeCount(int id, int count)
        {
            List<ShopCartItem> cartItems = new List<ShopCartItem>();
            if (Session["ShopCart"] != null)
            {
                cartItems = Session["ShopCart"] as List<ShopCartItem>;
            }

                if (cartItems.Any(p => p.ProductID == id))
                {
                    int index = cartItems.FindIndex(p => p.ProductID == id);
                if(count == 0)
                {
                    cartItems.RemoveAt(index);
                }
                else
                {
                    cartItems[index].Count = count;
                }
                }
            Session["ShopCart"] = cartItems;    
            return RedirectToAction("Orders");
        }

        public int ShopItemsCount()
        {
            List<ShopCartItem> cartItems = new List<ShopCartItem>();
            if (Session["ShopCart"] != null)
            {
                cartItems = Session["ShopCart"] as List<ShopCartItem>;
            }
            return cartItems.Sum(p => p.Count);
        }

        public int AddToCart(int? id)
        {
            List<ShopCartItem> cartItems = new List<ShopCartItem>();
            if (Session["ShopCart"] != null)
            {
                cartItems = Session["ShopCart"] as List<ShopCartItem>;
            }
            if (id != null && db.Products.Any(p => p.ProductID == id))
            {
                if (cartItems.Any(p => p.ProductID == id))
                {
                    int index = cartItems.FindIndex(p => p.ProductID == id);
                    cartItems[index].Count += 1;
                }
                else
                {
                    cartItems.Add(new ShopCartItem()
                    {
                        ProductID = id.Value,
                        Count = 1
                    });
                }
            }
            Session["ShopCart"] = cartItems;
            return ShopItemsCount();
        }

        public ActionResult ShowBasketSide()
        {
            List<ShopCartItemViewModel> cartItems = new List<ShopCartItemViewModel>();
            if (Session["ShopCart"] != null)
            {
               List<ShopCartItem> itemsList = Session["ShopCart"] as List<ShopCartItem>;
                foreach(var item in itemsList)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new { p.ProductImageName, p.ProductTitle}).Single();
                    cartItems.Add(new ShopCartItemViewModel()
                    {
                        ProductID =item.ProductID,
                        Count = item.Count,
                        Title = product.ProductTitle,
                        ImageName = product.ProductImageName

                    });
                }
            }
            return PartialView(cartItems);
        }
    }
}