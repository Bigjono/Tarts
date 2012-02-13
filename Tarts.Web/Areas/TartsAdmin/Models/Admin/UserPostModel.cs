using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Admin
{
    public class UserPostModel
    {
        public int ID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool Enabled { get; set; }
    }
}