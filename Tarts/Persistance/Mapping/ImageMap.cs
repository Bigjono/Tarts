using FluentNHibernate.Mapping;
using Tarts.Assets;

namespace Tarts.Persistance.Mapping
{
    public class ImageMap : ClassMap<Image>
    {

        public ImageMap()
        {
            Table("Image");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Name);
            Map(x => x.Created);
            Map(x => x.Large);
            Map(x => x.Medium);
            Map(x => x.Thumb);

            HasManyToMany(x => x.Galleries).Table("rel_images_to_gallery").ParentKeyColumn("ImageID").ChildKeyColumn("GalleryID")
               .Cascade.SaveUpdate().AsBag();
        }


    }
}
