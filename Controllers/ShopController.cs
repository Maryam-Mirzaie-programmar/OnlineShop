using Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            return PartialView(GetOrdersList());
        }

        public ActionResult ChangeCount(int id, int count)
        {
            List<ShopCartItem> cartItems = Session["ShopCart"] as List<ShopCartItem>;
            if (cartItems.Any(p => p.ProductID == id))
            {
                int index = cartItems.FindIndex(p => p.ProductID == id);
                if (count == 0)
                {
                    cartItems.RemoveAt(index);
                }
                else
                {
                    cartItems[index].Count = count;
                }
            }
            Session["ShopCart"] = cartItems;
            return PartialView("Orders", GetOrdersList());
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
                foreach (var item in itemsList)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new { p.ProductImageName, p.ProductTitle }).Single();
                    cartItems.Add(new ShopCartItemViewModel()
                    {
                        ProductID = item.ProductID,
                        Count = item.Count,
                        Title = product.ProductTitle,
                        ImageName = product.ProductImageName

                    });
                }
            }
            return PartialView(cartItems);
        }

        [Authorize]
        public ActionResult Payment()
        {
            int userId = db.Users.Single(u => u.Username == User.Identity.Name).UserId;
            var order = new Order()
            {
                UserId = userId,
                Date = DateTime.Now,
                IsFinally = false
            };
            db.Orders.Add(order);

            int amount = 0;
            var cartItems = GetOrdersList();
            foreach (var cartItem in cartItems)
            {
                db.OrderDetails.Add(new OrderDetail()
                {
                    ProductId = cartItem.ProductID,
                    Count = cartItem.Count,
                    OrderId = order.OrderId,
                    Price = cartItem.Price
                });
                amount += cartItem.Price * cartItem.Count;
            }
            db.SaveChanges();

            // Zarinpal Payment
            System.Net.ServicePointManager.Expect100Continue = false;
            Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();
            string Authority;

            int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", amount, "My Personal Description", "you@yoursite.com", "09123456789", "https://localhost:44338/Shop/Verify/" + order.OrderId, out Authority);

            if (Status == 100)
            {
                Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                ViewBag.Error = Status;
                Response.Write("error: " + Status);
            }

            return View();
        }

        public ActionResult Verify(int id)
        {
            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                if (Request.QueryString["Status"].ToString().Equals("OK"))
                {
                    var order = db.Orders.Find(id);
                    int Amount = order.OrderDetails.Sum(o => o.Count*o.Price);
                    long RefID;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    Zarinpal.PaymentGatewayImplementationServicePortTypeClient zp = new Zarinpal.PaymentGatewayImplementationServicePortTypeClient();

                    int Status = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Request.QueryString["Authority"].ToString(), Amount, out RefID);

                    if (Status == 100)
                    {
                        order.IsFinally = true;
                        db.SaveChanges();
                        Session["ShopCart"] = null;
                        ViewBag.RefID = RefID;
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ViewBag.Status = Status;
                        ViewBag.Success = false;
                    }

                }
                else
                {
                    ViewBag.Authority = Request.QueryString["Authority"].ToString();
                    ViewBag.Status = Request.QueryString["Status"].ToString();
                    ViewBag.Error = true;
                }
            }
            else
            {
                ViewBag.InvalidInput = true;
            }
            return View();
        }

        private List<OrderFactorViewModel> GetOrdersList()
        {
            List<OrderFactorViewModel> ordersList = new List<OrderFactorViewModel>();
            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> itemsList = Session["ShopCart"] as List<ShopCartItem>;
                foreach (var item in itemsList)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new { p.ProductImageName, p.ProductTitle, p.ProductPrice }).Single();
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
            return ordersList;
        }
    }
}