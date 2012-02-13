using FluentNHibernate.Mapping;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class EventMap : ClassMap<Event>
    {

        public EventMap()
        {
            Table("Event");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Name);
            Map(x => x.Slug);
            Map(x => x.Description);
            Map(x => x.StartTime);
            Map(x => x.EndTime);
            Map(x => x.TotalAllocation);
            Map(x => x.TotalSold);
            Map(x => x.Cancelled);
            Map(x => x.BookingConfirmation);


            HasManyToMany(x => x.Lineup).Table("rel_artist_to_event").ParentKeyColumn("EventID").ChildKeyColumn("ArtistID")
                .Cascade.SaveUpdate().AsBag();

            HasMany(x => x.Galleries).KeyColumn("EventID").Cascade.SaveUpdate();
            HasMany(x => x.Tickets).KeyColumn("EventID").Cascade.SaveUpdate();
            HasMany(x => x.Bookings).KeyColumn("EventID").Cascade.SaveUpdate();


        
        }


    }
}
