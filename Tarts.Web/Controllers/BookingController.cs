using System;
using System.Web;
using System.Web.Mvc;
using Bronson.Utils;
using Tarts.Bookings;
using Tarts.Config;
using Tarts.Customers;
using Tarts.Persistance;
using Tarts.Web.Infrastructure.Helpers;
using Tarts.Web.Models.Bookings;

namespace Tarts.Web.Controllers
{
    public class BookingController : Controller
    {
        private GenericRepo Repo;
        private Ticket Ticket;
        public BookingController(GenericRepo repo)
        {
            Repo = repo;
        }
        public ActionResult SoldOut()
        {
            return View();
        }
        public ActionResult CustomerAccount(string eventName, int ticketID)
        {
            Ticket = Repo.GetById<Ticket>(ticketID);
            if((Ticket == null) || (Ticket.Remaining < 1))
                return RedirectToAction("SoldOut");

            if (ViewBag.Customer != null)
            {
                var booking = new Booking(ViewBag.Customer, Ticket, 1);
                Repo.Save(booking);
                return RedirectToAction("ReservationSummary", new { id = booking.ID });
            }

            return View(new CreateCustomerViewModel(Ticket));
        }
       
        [HttpPost]
        public ActionResult CustomerAccount(CreateCustomerPostModel model)
        {
            Ticket = Repo.GetById<Ticket>(model.ticketID);
            if ((Ticket == null) || (Ticket.Remaining < 1)) return RedirectToAction("SoldOut");

            var existing = Repo.GetByFieldName<Customer>("Email", model.Email);
            if (existing != null)
            {
                ViewBag.ErrorMessage = "A user already exists with that email address";
                return View(new CreateCustomerViewModel(Ticket,model));
            }

            var cust = new Customer(model.Email, model.FirstName, model.Surname, model.Password);
            var valid = cust.Validate();
            if(!valid.Succeeded)
            {
                ViewBag.ErrorMessage =  valid.Message;
                return View(new CreateCustomerViewModel(Ticket, model)); 
            }
            Repo.Save(cust);
            var booking = new Booking(cust, Ticket, 1);
            Repo.Save(booking);
            return RedirectToAction("ReservationSummary", new {id=booking.ID});
        }

        [HttpPost]
        public ActionResult AccountLogin(CustomerLoginPostModel model)
        {
            Ticket = Repo.GetById<Ticket>(model.ticketID);
            if ((Ticket == null) || (Ticket.Remaining < 1))
                return RedirectToAction("SoldOut");
            var existing = Repo.GetByFieldName<Customer>("Email", model.Email);
            if (existing != null)
            {
                if (existing.Password.Decrypt() == model.Password)
                {
                    var cookie = new HttpCookie("TartsUser", existing.ID.EncryptInteger());
               
                    cookie.Expires = DateTime.Now.AddDays(1);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    Request.Cookies.Add(cookie);
                    var booking = new Booking(existing, Ticket, 1);
                    Repo.Save(booking);
                    return RedirectToAction("ReservationSummary", new { id = booking.ID });
                }
                ViewBag.ErrorMessage = "Login Failed";
            }
            else
                ViewBag.ErrorMessage = "No customer account was found for that email address";
            return View(new CreateCustomerViewModel(Ticket));
        }

        public ActionResult ReservationSummary(int id)
        {
            var booking = Repo.GetById<Booking>(id);
            return View(booking);
        }

        [HttpPost]
        public ActionResult ReservationSummary(AmmendBookingPostModel model)
        {
            var booking = Repo.GetById<Booking>(model.bookingID);
            booking.UpdateQuantity(model.Quantity);
            Repo.Save(booking);
            return View(booking);
        }

        [HttpPost]
        public ActionResult MakePayment(string id)
        {
            Settings cfg = ViewBag.SiteSettings;
            var booking = Repo.GetById<Booking>(id.DecryptInteger());

            if (booking.Total > 0)
            {
                var canMakePayment = booking.CanMakePayment();
                if (canMakePayment.Succeeded)
                {
                    var payment = booking.AddPayment();
                    Repo.Save(payment);

                    var myremotepost = new RemotePost { Url = cfg.PaypalUrl };
                    myremotepost.Add("cmd", "_xclick");
                    myremotepost.Add("business", cfg.PaypalUsername);
                    myremotepost.Add("currency_code", "GBP");
                    myremotepost.Add("item_name", booking.Ticket.Name);
                    myremotepost.Add("amount", booking.Total.ToString());
                    myremotepost.Add("return", "~/payments/paymentresult".ToFullSitePath());
                    myremotepost.Add("custom", ("tarts_" + payment.ID).Encrypt());
                    myremotepost.Post();
                    return View(booking);
                }
                else
                {
                    ViewBag.ErrorMessage = canMakePayment.Message;
                    RedirectToAction("ReservationSummary", new { id = booking.ID });
                }
            }
            return RedirectToAction("BookingComplete", new {id = booking.ID});
        }

        [HttpPost]
        public ActionResult BookingComplete(int id)
        {
            var booking = Repo.GetById<Booking>(id);

            return View(booking);
        }


        
        
    }
}
