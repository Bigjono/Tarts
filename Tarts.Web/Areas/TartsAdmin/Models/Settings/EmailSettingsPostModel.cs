using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Settings
{
    public class EmailSettingsPostModel
    {
        public virtual string SmtpHost { get; set; }
        public virtual string SmtpUsername { get; set; }
        public virtual string SmtpPassword { get; set; }
        public virtual string ForceEmailsTo { get; set; }
        public virtual string EmailFromName { get; set; }
        public virtual string EmailFromAddress { get; set; }
    }
}