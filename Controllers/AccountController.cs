using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datalayer;
using System.Web.Security;

namespace MyEshop.Controllers
{
    public class AccountController : Controller
    {
        OnlineShopDBEntities db = new OnlineShopDBEntities();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                if (! db.Users.Any(u => u.Email == register.Email.Trim().ToLower()))
                {
                    db.Users.Add(new Datalayer.User()
                    {
                        RoleId = 1,
                        Username = register.Username,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password , "MD5"),
                        Email = register.Email.Trim().ToLower(),
                        ActrivationCode = Guid.NewGuid().ToString(),
                        IsActive = false,
                        RegisterDate = DateTime.Now
                    });
                    db.SaveChanges();
                    var user = db.Users.Single(u => u.Email == register.Email.Trim());

                    // ToDo: Send Activation Email to User

                    return View("SuccessRegister" , user);
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری می باشد.");
                }
            }
            return View(register);
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}