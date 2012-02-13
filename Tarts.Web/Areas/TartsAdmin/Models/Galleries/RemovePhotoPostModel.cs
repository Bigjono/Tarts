namespace Tarts.Web.Areas.TartsAdmin.Models.Galleries
{
    public class RemovePhotoPostModel
    {
        public int GalleryID { get; set; }
        public int PhotoID { get; set; }
        public bool DeletePhoto { get; set; }
    }
}