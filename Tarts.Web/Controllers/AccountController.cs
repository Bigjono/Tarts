using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Customers;
using Tarts.Persistance;
using Tarts.Web.Models.Account;

namespace Tarts.Web.Controllers
{
    public class AccountController : Controller
    {
        private GenericRepo Repo;
        public AccountController(GenericRepo repo)
        {
            Repo = repo;
        }

        public ActionResult LogIn()
        {
            return View();
        }


        public ActionResult LogOut()
        {
            //FormsAuthentication.SignOut();
            return Redirect("/");
        }

        //[HttpPost]
        //public ActionResult LogOnIn(LoginPostModel model, string returnUrl = "/")
        //{
           
        //        var cust = Repo.GetByFieldName<Customer>("Email", model.Email);
        //        if ((cust != null) && (cust.Password.Decrypt() == model.Password))
        //        {
        //            FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);
        //            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
        //                return Redirect(returnUrl);
        //            else
        //                return RedirectToAction("Index", "Pages", new { area = "TartsAdmin" });
        //        }
        //        else
        //            ModelState.AddModelError("", "The user name or password provided is incorrect.");

        //    return Redirect(returnUrl);
        //}

    }
}
