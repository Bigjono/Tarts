using System;
using Bronson.Utils;
using Tarts.Base;

namespace Tarts.Admin
{
    public class User : EntityBase
    {
        public virtual string FirstName { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool Enabled { get; set; }

        public User (){}
        public User (string email, string password)
        {
            Email = email;
            Password = password.Encrypt();
            Enabled = true;
        }

        public virtual void Update(string firstname, string surname)
        {
            FirstName = firstname;
            Surname = surname;
        }
        public virtual ReturnValue ChangeEmail(string email)
        {
            if(!email.ValidateEmail()) return new ReturnValue(false, "Email address appear to be invalid");
            Email = email;
            return new ReturnValue();
        }
        public virtual ReturnValue ChangePassword(string password)
        {
            if (!password.ValidatePassword()) return new ReturnValue(false, "Password is too weak");
            Password = password.Encrypt();
            return new ReturnValue();
        }
        public virtual void EnabledUser()
        {
            Enabled = true;
        }
        public virtual void DisableUser()
        {
            Enabled = false;
        }
        public virtual ReturnValue Validate()
        {
            if (!Email.ValidateEmail()) return new ReturnValue(false, "Email address appear to be invalid");
            if (!Password.Decrypt().ValidatePassword()) return new ReturnValue(false, "Password is too weak");
            return new ReturnValue();
        }

    }
}
