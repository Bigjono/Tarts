using FluentNHibernate.Mapping;
using Tarts.Bookings;
using Tarts.Customers;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class BookingMap : ClassMap<Booking>
    {

        public BookingMap()
        {
            Table("Booking");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Reference);
            Map(x => x.Created);
            Map(x => x.Quantity);
            Map(x => x.TicketPrice);
            Map(x => x.BookingFee);
            Map(x => x.TotalPaid);
            Map(x => x.DiscountApplied);
            Map(x => x.VoucherCodeApplied);
            Map(x => x.Status).CustomType(typeof(Booking.BookingStatus)).Default((Booking.BookingStatus.Reservation.ToString()));

            References(x => x.Voucher).Column("VoucherID").NotFound.Ignore();
            References(x => x.Event).Column("EventID").NotFound.Ignore();
            References(x => x.Customer).Column("CustomerID").NotFound.Ignore();
            References(x => x.Ticket).Column("TicketID").NotFound.Ignore();
            HasMany(x => x.Payments).KeyColumn("BookingID").Cascade.SaveUpdate();


        }


    }
}
