using FluentNHibernate.Mapping;
using Tarts.Bookings;
using Tarts.Customers;
using Tarts.Ecommerce;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class PaymentMap : ClassMap<Payment>
    {

        public PaymentMap()
        {
            Table("Payment");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Reference);
            Map(x => x.Created);
            Map(x => x.Amount);
            Map(x => x.Message);
            Map(x => x.Details);
            Map(x => x.Status).CustomType(typeof(Booking.BookingStatus)).Default((Booking.BookingStatus.Reservation.ToString()));

            References(x => x.Booking).Column("BookingID").NotFound.Ignore();

        }


    }
}
