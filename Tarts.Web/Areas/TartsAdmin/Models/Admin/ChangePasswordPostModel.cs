using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Admin
{
    public class ChangePasswordPostModel
    {
        public int ID { get; set; }
        public virtual string Password { get; set; }
    }
}