using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Models.Bookings
{
    public class CreateCustomerPostModel
    {
        public string eventName { get; set; }
        public int ticketID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
    }
}