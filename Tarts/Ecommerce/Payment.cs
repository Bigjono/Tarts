using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tarts.Base;
using Tarts.Bookings;
using Tarts.Customers;

namespace Tarts.Ecommerce
{
    public class Payment : EntityBase
    {
        public enum PaymentStatus : int
        {
            Created = 1,
            Sent = 2,
            Verified = 3,
            Complete = 4,
            Failed = 5,
            Refunded = 6
        }


        public virtual PaymentStatus Status { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual string Reference { get; set; }
        public virtual string Message { get; set; }
        public virtual string Details { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual Booking Booking { get; set; }

        public Payment() { }
        public Payment(Booking booking, decimal amount, string reference)
        {
            Booking = booking;
            Amount = amount;
            Reference = reference;
            Status = PaymentStatus.Created;
            Created = DateTime.Now;
        }

        public virtual void MarkAsSent()
        {
            Status = PaymentStatus.Sent;
        }
       
    }
}
