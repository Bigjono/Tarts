using FluentNHibernate.Mapping;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class ArtistMap : ClassMap<Artist>
    {

        public ArtistMap()
        {
            Table("Artist");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Name);
            Map(x => x.Slug);
            Map(x => x.Bio);

            References(x => x.Image).Column("ImageID").NotFound.Ignore();

        }


    }
}
