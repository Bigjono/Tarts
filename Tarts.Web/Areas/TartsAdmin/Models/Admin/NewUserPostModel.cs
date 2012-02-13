using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Admin
{
    public class NewUserPostModel
    {
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
    }
}