using FluentNHibernate.Mapping;
using Tarts.Bookings;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class TicketMap : ClassMap<Ticket>
    {

        public TicketMap()
        {
            Table("Ticket");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Name);
            Map(x => x.Allocation);
            Map(x => x.Sold);
            Map(x => x.Price);
            Map(x => x.BookingFee);
            Map(x => x.Enabled);

            References(x => x.Event).Column("EventID").NotFound.Ignore();

        }


    }
}
