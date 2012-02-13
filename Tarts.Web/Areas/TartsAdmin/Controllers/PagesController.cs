using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarts.Content;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Pages;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class PagesController : Controller
    {

        private GenericRepo Repo;
        public PagesController(GenericRepo repo)
        {
            Repo = repo;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(Repo.GetAll<Page>());
        }

        [Authorize]
        public ActionResult New()
        {
            return View(new Page());
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = Repo.GetById<Page>(id);
            return View(model);
        }

        [ValidateInput(false),Authorize]
        public ActionResult Update(PagePostModel model)
        {
            var usr = (model.ID == 0) ? new Page() : Repo.GetById<Page>(model.ID);
            UpdateModel(usr, model);
            return RedirectToAction("Index");
            //return RedirectToAction("Edit", new { id = model.ID });
        }
        [Authorize]
        public ActionResult Destroy(int id)
        {
            var usr = Repo.GetById<Page>(id);
            Repo.Delete(usr);
            return RedirectToAction("Index");
        }

        private void UpdateModel(Page user, PagePostModel model)
        {
            user.Name = model.Name;
            user.Content = model.Content;
            Repo.Save(user);
        }
    }
}
