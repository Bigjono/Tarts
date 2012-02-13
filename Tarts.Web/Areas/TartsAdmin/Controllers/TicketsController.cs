using System.Web.Mvc;
using Tarts.Admin;
using Tarts.Bookings;
using Tarts.Events;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Models.Events;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class TicketsController : Controller
    {

        private GenericRepo Repo;
        private Ticket Ticket;
        public TicketsController(GenericRepo repo)
        {
            Repo = repo;
        }

       
        [Authorize]
        public ActionResult New (NewTicketPostModel model = null)
        {
            if (model == null) model = new NewTicketPostModel();
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var model = Repo.GetById<Ticket>(id);
            return View(model);
        }

        [Authorize]
        public ActionResult Create(NewTicketPostModel model)
        {
            var obj = Repo.GetByFieldName<Event>("Slug", model.EventSlug);
            var child = obj.AddTicket(model.Name,model.Price);
            Repo.Save(child);
            return RedirectToAction("Edit", new { id = child.ID });
        }

        [Authorize]
        public ActionResult Disable(int id)
        {
            var model = Repo.GetById<Ticket>(id);
            model.Event.DisableTicket(model);
            Repo.Save(model.Event);
            return RedirectToAction("Edit", new { id = model.ID });
        }

        [Authorize]
        public ActionResult Enable(int id)
        {
            var model = Repo.GetById<Ticket>(id);
            model.Event.EnableTicket(model);
            Repo.Save(model.Event);
            return RedirectToAction("Edit", new { id = model.ID });
        }

        [Authorize]
        public ActionResult Update(TicketPostModel model)
        {
            var obj = Repo.GetById<Ticket>(model.ID);
            if (obj == null)
            {
                TempData.Add("ErrorMessage", "Failed to find ticket");
                return RedirectToAction("Index", "Events");
            }
            var res = obj.Event.UpdateTicketInfo(obj, model.Name, model.Price, model.BookingFee, model.Allocation, model.Enabled);
            if(res.Succeeded)
            {
                TempData.Add("Message", obj.Name + " Ticket Updated");
                Repo.Save(obj.Event);
                return RedirectToAction("Edit", "Events",new { id = obj.Event.ID } );
            }
            TempData.Add("ErrorMessage", res.Message);
            return RedirectToAction("Edit", model.ID);
        }

        

      
    }
}
