using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Models.Account
{
    public class CreateCustomerPostModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
    }
}