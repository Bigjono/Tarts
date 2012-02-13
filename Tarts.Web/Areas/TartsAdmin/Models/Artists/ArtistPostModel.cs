using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Artists
{
    public class ArtistPostModel
    {
        public int ID { get; set; }
        public int ImageID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Bio { get; set; }
    }
}