using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Models.Account
{
    public class LoginPostModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}