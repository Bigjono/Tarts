
namespace Tarts.Web.Areas.TartsAdmin.Models.Events
{
    public class NewTicketPostModel
    {
        public virtual string EventSlug { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
    }
}