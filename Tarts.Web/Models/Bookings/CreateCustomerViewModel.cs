using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarts.Bookings;

namespace Tarts.Web.Models.Bookings
{
    public class CreateCustomerViewModel
    {

        public CreateCustomerViewModel(Ticket ticket = null, CreateCustomerPostModel postModel = null)
        {
            if (ticket != null)
            {
                EventName = ticket.Event.Name;
                EventSlug = ticket.Event.Slug;
                TicketID = ticket.ID;
                TicketName = ticket.Name;
            }
            if(postModel != null)
            {
                FirstName = postModel.FirstName;
                Surname = postModel.Surname;
                Email = postModel.Email;
            }
        }

        public string EventName { get; set; }
        public string EventSlug { get; set; }
        public int TicketID { get; set; }
        public string TicketName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
    }
}