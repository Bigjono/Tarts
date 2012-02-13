using System;
using System.Web.Mvc;
using Tarts.Assets;
using Tarts.Events;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Artists;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class ArtistsController : Controller
    {

        private GenericRepo Repo;
        public ArtistsController(GenericRepo repo)
        {
            Repo = repo;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(Repo.GetAll<Artist>());
        }

        [Authorize]
        public ActionResult New()
        {
            return View(new Artist());
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = Repo.GetById<Artist>(id);
            return View(model);
        }

        [ValidateInput(false),Authorize]
        public ActionResult Update(ArtistPostModel model)
        {
            var obj = (model.ID == 0) ? new Artist() : Repo.GetById<Artist>(model.ID);
            UpdateModel(obj, model);
            return RedirectToAction("Index");
            //return RedirectToAction("Edit", new { id = model.ID });
        }
        [Authorize]
        public ActionResult Destroy(int id)
        {
            var usr = Repo.GetById<Artist>(id);
            Repo.Delete(usr);
            return RedirectToAction("Index");
        }

        private void UpdateModel(Artist obj, ArtistPostModel model)
        {
            obj.UpdateInfo(model.Name,model.Slug,model.Bio);
            obj.SetImage(Repo.GetById<Image>(model.ImageID));
            Repo.Save(obj);
        }
    }
}
