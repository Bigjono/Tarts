using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Settings
{
    public class PaymentSettingsPostModel
    {
        public virtual string PaypalUrl { get; set; }
        public virtual string PaypalPdtToken { get; set; }
        public virtual string PaypalUsername { get; set; }
    }
}