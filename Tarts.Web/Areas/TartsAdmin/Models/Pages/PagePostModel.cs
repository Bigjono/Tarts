using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Pages
{
    public class PagePostModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}