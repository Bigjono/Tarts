using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Content;
using Tarts.Persistance;

namespace Tarts.Web.Controllers
{
    public class PageController : Controller
    {
        //
        private GenericRepo Repo;
        public PageController(GenericRepo repo)
        {
            Repo = repo;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult ChoosePage(string slug)
        {
            return View(Repo.GetByFieldName<Page>("Name", slug));
        }

    }
}
