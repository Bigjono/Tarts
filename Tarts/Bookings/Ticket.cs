using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bronson.Utils;
using Tarts.Base;
using Tarts.Events;

namespace Tarts.Bookings
{
    public class Ticket : EntityBase
    {

        public virtual string Name { get; set; }
        public virtual int Allocation { get; set; }
        public virtual int Sold { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal BookingFee { get; set; }
        public virtual bool Enabled { get; set; }
                
        public virtual Event Event { get; set; }

        public virtual string PriceDescription { get { return (Price > 0m) ? Price.ToCurrency() : "Free"; } }

        public Ticket()
        {
            Enabled = true;
            BookingFee = 3.5m;
            Allocation = 100;
        }
        
        public virtual ReturnValue UpdateTicketInfo(string name, decimal price, decimal bookingFee, int allocation,  bool enabled)
        {
            if ((Allocation != allocation) && (allocation < Sold)) return new ReturnValue(false, "You cannot decrease to allocation to less than the amount already sold");
            Name = name;
            Price = price;
            BookingFee = bookingFee;
            Allocation = allocation;
            Enabled = enabled;
            return new ReturnValue();
        }
        


        public virtual void Disable()
        {
            Enabled = false;
        }
        public virtual void Enable()
        {
            Enabled = true;
        }
        public virtual decimal Cost
        {
            get { return Price + BookingFee; }
        }
        public virtual int Remaining
        {
            get { return ((Allocation - Sold) < 0) ? 0 : Allocation - Sold; }
        }

        public virtual void ReCalculateTicketsSold()
        {
            //Sold = Event.Bookings.Where(booking => ((booking.Status == Booking.BookingStatus.Complete) && (booking.Ticket == this))).Sum(booking => booking.Quantity);
            Sold = 0;
            foreach (var booking in Event.Bookings.Where(x => x.Ticket.ID == ID))
            {
                if (booking.Status == Booking.BookingStatus.Complete)
                    Sold += booking.Quantity;
            }

            this.Event.ReCalculateTicketTotals();
        }
        
    }
}
