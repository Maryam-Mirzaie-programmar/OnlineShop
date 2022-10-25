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

        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
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

                    // Send Activation Email to User
                    try
                    {
                        var body = PartialToStringClass.RenderPartialView("ManageEmails", "ActivationEmail", user);
                        SendEmail.Send(user.Email, "ایمیل فعالسازی", body);
                        return View("SuccessRegister", user);
                    }
                    catch
                    {
                        db.Users.Remove(user);
                        db.SaveChanges();
                        ModelState.AddModelError("Email", "متاسفانه در ارسال ایمیل فعالسازی خطایی رخ داده است.");
                    }    
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری می باشد.");
                }
            }
            return View(register);
        }

        public ActionResult ActiveUser()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}