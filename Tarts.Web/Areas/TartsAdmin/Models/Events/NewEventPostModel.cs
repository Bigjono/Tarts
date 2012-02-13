using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Events
{
    public class NewEventPostModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public NewEventPostModel()
        {
            StartTime = DateTime.Now.AddMonths(7);
            EndTime = DateTime.Now.AddMonths(7);
        }
    }
}