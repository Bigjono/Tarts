using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tarts.Admin;
using Tarts.Customers;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Account;
using Tarts.Web.Models.Account;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class AccountController : Controller
    {

        private GenericRepo Repo;
        public AccountController(GenericRepo repo)
        {
            Repo = repo;
        }

        public ActionResult LogOn()
        {
            return View();
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Logon", "Account");
        }

        [HttpPost]
        public ActionResult LogOn(LogonModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = Repo.GetByFieldName<User>("Email", model.UserName);
                if ((user != null) && (user.Password.Decrypt() == model.Password.Decrypt()) && (user.Enabled))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Pages", new { area = "TartsAdmin" });
                }
                else
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");

            }
            return View(model);
        }

       

    }
}
