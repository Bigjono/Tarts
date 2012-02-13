using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Content;
using Tarts.Persistance;

namespace Tarts.Web.Controllers
{
    public class HomeController : Controller
    {
        private GenericRepo Repo;
        public HomeController(GenericRepo repo)
        {
            Repo = repo;
        }

        public ActionResult Index()
        {
            //return RedirectToAction("Index", "Pages",new { area = "TartsAdmin" });
            return View(Repo.GetByFieldName<Page>("Name", "Home") ?? new Page());
        }

    }
}
