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