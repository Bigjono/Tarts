using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Events
{
    public class TicketPostModel
    {
        public int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal BookingFee { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual int Allocation { get; set; }
    }
}