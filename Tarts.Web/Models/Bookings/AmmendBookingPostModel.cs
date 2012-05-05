using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Models.Bookings
{
    public class AmmendBookingPostModel
    {
        public int bookingID { get; set; }
        public int Quantity { get; set; }
        public string VoucherCode { get; set; }
    }
}