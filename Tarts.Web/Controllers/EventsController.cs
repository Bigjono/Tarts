using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Content;
using Tarts.Events;
using Tarts.Persistance;

namespace Tarts.Web.Controllers
{
    public class EventsController : Controller
    {
        private GenericRepo Repo;
        public EventsController(GenericRepo repo)
        {
            Repo = repo;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult View(string id)
        {
            var g = Repo.GetByFieldName<Event>("Slug", id);
            return View(g);
        }

        public ViewResult Lineup(string id)
        {
            var g = Repo.GetByFieldName<Event>("Slug", id);
            return View(g);
        }

        public ViewResult Tickets(string id)
        {
            var g = Repo.GetByFieldName<Event>("Slug", id);
            return View(g);
        }

        
    }
}
