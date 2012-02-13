using System.Web.Mvc;
using Tarts.Assets;
using Tarts.Content;
using Tarts.Events;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Galleries;
using Tarts.Web.Infrastructure.Helpers;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class GalleriesController : Controller
    {

        private GenericRepo Repo;
        private ImageHelper ImageHelper;
        public GalleriesController(GenericRepo repo, ImageHelper imageHelper)
        {
            Repo = repo;
            ImageHelper = imageHelper;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View(Repo.GetAll<Gallery>());
        }

        [Authorize]
        public ActionResult New()
        {
            ViewBag.Events = Repo.GetAll<Event>("Name");
            return View(new Gallery());
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = Repo.GetById<Gallery>(id);
            ViewBag.Events = Repo.GetAll<Event>("Name");
            return View(model);
        }
        [Authorize]
        public ActionResult Update(GalleryPostModel model)
        {
            var obj = (model.ID == 0) ? new Gallery() : Repo.GetById<Gallery>(model.ID);
            UpdateModel(obj, model);
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Destroy(int id)
        {
            var obj = Repo.GetById<Gallery>(id);
            Repo.Delete(obj);
            return RedirectToAction("Index");
        }

        private void UpdateModel(Gallery obj, GalleryPostModel model)
        {
            var evt = (model.EventID.HasValue) ? Repo.GetById<Event>(model.EventID.Value) : null;
            obj.Update(model.Name, model.Slug, model.Date,evt);
            obj.SetDefaultImage(Repo.GetById<Image>(model.DefaultImageID));
            Repo.Save(obj);
        }
        [Authorize]
        public ActionResult RemovePhoto(RemovePhotoPostModel model)
        {
            var gallery = Repo.GetById<Gallery>(model.GalleryID);
            var photo = Repo.GetById<Image>(model.PhotoID);
            if(gallery!=null)
            {
                gallery.RemovePhoto(photo);
                Repo.Save(gallery);
                if(model.DeletePhoto) 
                    ImageHelper.Delete(Server, photo);
            }
            return RedirectToAction("Edit", new { id = model.GalleryID });
        }
    }
}
