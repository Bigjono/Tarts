using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Galleries
{
    public class GalleryPostModel
    {
        public int ID { get; set; }
        public int DefaultImageID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime Date { get; set; }
        public int? EventID { get; set; }
    }
}