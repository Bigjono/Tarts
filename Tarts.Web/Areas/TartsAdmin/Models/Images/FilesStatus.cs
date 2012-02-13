﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Images
{
    public class FilesStatus
    {
        public string thumbnail_url { get; set; }
        public string name { get; set; }
        public string alt_text { get; set; }
        public string url { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string update_url { get; set; }
        public string update_type { get; set; }
        public string error { get; set; }
        public string progress { get; set; }
    }
}