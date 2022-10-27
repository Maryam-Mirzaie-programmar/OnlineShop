using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Datalayer;
using System.Web.Security;
using System.Data.Entity;

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
                if (!db.Users.Any(u => u.Email == register.Email.Trim().ToLower()))
                {
                    db.Users.Add(new Datalayer.User()
                    {
                        RoleId = 1,
                        Username = register.Username,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
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
                        if(User.Identity.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }
                        return View("SuccessRegister", user);
                    }
                    catch
                    {
                        db.Users.Remove(user);
                        db.SaveChanges();
                        ModelState.AddModelError("Email", "متاسفانه در ارسال ایمیل  خطایی رخ داده است.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری می باشد.");
                }
            }
            return View(register);
        }

        public ActionResult ActiveUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var user = db.Users.SingleOrDefault(u => u.ActrivationCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsActive = true;
            user.ActrivationCode = Guid.NewGuid().ToString();
            db.SaveChanges();

            ViewBag.name = user.Username;
            return View();
        }

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login ,string ReturnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = db.Users.SingleOrDefault(u => u.Email == login.Email.Trim().ToLower() && u.Password == hashPassword);
                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Username, login.RememberMe);
                    return Redirect(ReturnUrl);
                }
            }

            return View(login);
        }



        [Route("ForgetPassword")]
        public ActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ForgetPassword")]
        public ActionResult ForgetPassword(ForgetPasswordViewModel forget)
        {
            if(ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Email == forget.Email.Trim().ToLower());
                if(user != null && user.IsActive)
                {
                    // Send Reset Password Link Email to User
                    try
                    {
                        var body = PartialToStringClass.RenderPartialView("ManageEmails", "ResetPassword",user);
                        SendEmail.Send(user.Email, "بازیابی کلمه عبور", body);
                        return View("SuccessForgetPassword" , user);
                    }
                    catch
                    {
                        ModelState.AddModelError("Email", "متاسفانه در ارسال ایمیل  خطایی رخ داده است.");
                    }
                }
                ModelState.AddModelError("Email", "حساب فعالی با این ایمیل یافت نشد");
            }
            return View(forget);
        }


        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var user = db.Users.SingleOrDefault(u => u.ActrivationCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel reset, string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                var user = db.Users.SingleOrDefault(u => u.ActrivationCode == id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(reset.Password, "MD5");
                user.Password = hashPassword;
                user.ActrivationCode = Guid.NewGuid().ToString();
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut(); 
                }
                return RedirectToAction("Login",new { ResetPassword  = true});
            }
            
            return View(reset);
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}