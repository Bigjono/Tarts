using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Tarts.Content;

namespace Tarts.Persistance.Mapping
{
    public class GalleryMap : ClassMap<Gallery>
    {

        public GalleryMap()
        {
            Table("Gallery");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Name);
            Map(x => x.Slug);
            Map(x => x.Date);

            References(x => x.DefaultImage).Column("DefaultImageID").NotFound.Ignore();
            References(x => x.Event).Column("EventID").NotFound.Ignore();

            HasManyToMany(x => x.Photos).Table("rel_images_to_gallery").ParentKeyColumn("GalleryID").ChildKeyColumn("ImageID")
                .Cascade.SaveUpdate().AsBag();
        }


    }
}
