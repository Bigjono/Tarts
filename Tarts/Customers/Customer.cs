using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bronson.Utils;
using Tarts.Base;

namespace Tarts.Customers
{
    public class Customer : EntityBase
    {

        public enum Sex : int
        {
            Male = 1,
            Female = 2
        }

        public virtual string FirstName { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Mobile { get; set; }
        public virtual bool Subscribed { get; set; }
        public virtual Sex Gender { get; set; }


        public Customer() {}
        public Customer(string email, string firstName, string surname, string plainPassword)
        {
            FirstName = firstName;
            Surname = surname;
            Email = email;
            Password = plainPassword.Encrypt();
            Subscribed = true;
        }


        public virtual ReturnValue Validate()
        {
            if (string.IsNullOrWhiteSpace(FirstName)) return new ReturnValue(false,"No First Name Provided");
            if (string.IsNullOrWhiteSpace(Surname)) return new ReturnValue(false,"No Surname Provided");
            if (!Email.ValidateEmail()) return new ReturnValue(false,"Email address is invalid");
            if (!Password.Decrypt().ValidatePassword()) return new ReturnValue(false,"Password is invalid");
            return new ReturnValue();
        }

    }
}
