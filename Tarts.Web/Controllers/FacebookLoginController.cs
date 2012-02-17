using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook.Web;

namespace Tarts.Web.Controllers
{
    public class FacebookLoginController : Controller
    {
        //
        // GET: /FacebookLogin/

        public ActionResult Index()
        {
            if (FacebookWebContext.Current.IsAuthenticated())
            {
                var client = new FacebookWebClient();

                dynamic me = client.Get("me");
                ViewBag.Name = me.name;
                ViewBag.Id = me.id;
                ViewBag.Email = me.email;
            }
            return View();
        }

    }
}
