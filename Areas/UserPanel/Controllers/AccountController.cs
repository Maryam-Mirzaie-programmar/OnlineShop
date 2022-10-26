using Datalayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyEshop.Areas.UserPanel.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        OnlineShopDBEntities db = new OnlineShopDBEntities();

        // GET: UserPanel/Account
        public ActionResult ChangePassword()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var user = db.Users.Single(u => u.Username == username);
                string oldHashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "MD5");
                if(oldHashPassword == user.Password)
                {
                    string newHashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(change.NewPassword, "MD5");
                    user.Password = newHashPassword;
                    db.SaveChanges();
                    FormsAuthentication.SignOut();
                    return Redirect("/Login?ResetPassword=true");
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "رمز عور وارد شده صحیح نمی باشد");
                }
            }
            return View(change);
        }
    }
}