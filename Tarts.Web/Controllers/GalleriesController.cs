using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Content;
using Tarts.Persistance;

namespace Tarts.Web.Controllers
{
    public class GalleriesController : Controller
    {
         private GenericRepo Repo;
         public GalleriesController(GenericRepo repo)
        {
            Repo = repo;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult View(string id)
        {
            var g = Repo.GetByFieldName<Gallery>("Slug", id);
            return View(g);
        }


    }
}
