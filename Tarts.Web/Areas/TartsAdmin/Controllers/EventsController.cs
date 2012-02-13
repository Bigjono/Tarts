using System;
using System.Linq;
using System.Web.Mvc;
using Tarts.Bookings;
using Tarts.Content;
using Tarts.Events;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Events;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class EventsController : Controller
    {

        private GenericRepo Repo;
        public EventsController(GenericRepo repo)
        {
            Repo = repo;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View(Repo.GetAll<Event>());
        }

        [Authorize]
        public ActionResult New(NewEventPostModel model = null)
        {
            if (model == null) model = new NewEventPostModel();
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = Repo.GetById<Event>(id);
            ViewBag.Artists = Repo.GetAll<Artist>("Name");
            return View(model);
        }

        [Authorize]
        public ActionResult Create(NewEventPostModel model)
        {
            var obj = new Event(model.Name,model.Slug,model.StartTime,model.EndTime);
            var checkCreds = obj.Validate();
            if (checkCreds.Succeeded)
            {
                Repo.Save(obj);
                return RedirectToAction("Edit", new { id = obj.ID });
            }
            TempData.Add("ErrorMessage", checkCreds.Message);
            return RedirectToAction("New", model);
        }

        [ValidateInput(false),Authorize]
        public ActionResult Update(EventPostModel model)
        {
            var obj = (model.ID == 0) ? new Event() : Repo.GetById<Event>(model.ID);
            var res = obj.Update(model.Name, model.Slug, model.Description, model.StartTime, model.EndTime, model.BookingConfirmation);
            var artistKeys = Request.Form.AllKeys.Where(k => k.StartsWith("artist_")).ToList();
            obj.RemoveArtists();
            foreach (var key in artistKeys)
            {
                var art = Repo.GetById<Artist>(key.Replace("artist_", "").ConvertToInt32(0));
                obj.AddArtist(art);
            }
            if(res.Succeeded)
            {
                Repo.Save(obj);
                return RedirectToAction("Index");   
            }
            TempData.Add("ErrorMessage", res.Message);
            return RedirectToAction("Edit", model);
        }
        [Authorize]
        public ActionResult Destroy(int id)
        {
            var obj = Repo.GetById<Event>(id);
            var res = obj.CanDelete();
            if(res.Succeeded)
            {
                Repo.Delete(obj);
                return RedirectToAction("Index");
            }
           
            TempData.Add("ErrorMessage", res.Message);
            return RedirectToAction("Edit", new { id = id });
            
            
        }

        [Authorize]
        public ActionResult RemoveGallery(RemoveGalleryPostModel model)
        {
            var evt = Repo.GetById<Event>(model.EventID);
            var gallery = Repo.GetById<Gallery>(model.GalleryID);
            if (evt != null)
            {
                evt.RemoveGallery(gallery);
                Repo.Save(evt);
            }
            return RedirectToAction("Edit", new { id = model.EventID });
        }

        [Authorize]
        public ActionResult RemoveTicket(RemoveTicketPostModel model)
        {
            var evt = Repo.GetById<Event>(model.EventID);
            var ticket = Repo.GetById<Ticket>(model.TicketID);
            if (evt != null)
            {
                if(evt.TotalSold > 0)
                    TempData["Message"] = "Ticket disabled as some tickets have already been sold";
                evt.RemoveTicket(ticket);
                Repo.Save(evt);
            }
            return RedirectToAction("Edit", new { id = model.EventID });
        }

       
    }
}
