using System;
using System.Collections.Generic;
using Bronson.Utils;
using Tarts.Base;
using Tarts.Customers;
using Tarts.Ecommerce;
using Tarts.Events;

namespace Tarts.Bookings
{
    public class Booking : EntityBase
    {

        public enum BookingStatus : int
        {
            Reservation = 1,
            Complete = 2,
            Cancelled = 3
        }

        public Booking()
        {
            Status = BookingStatus.Reservation;
            Quantity = 1;
            Payments = new List<Payment>();
            Created = DateTime.Now;
        }
        public Booking(Customer customer, Ticket ticket, int quantity)
        {
            Created = DateTime.Now;
            Status = BookingStatus.Reservation;
            Quantity = (quantity > 0) ? quantity : 1;
            Customer = customer;
            Ticket = ticket;
            TicketPrice = Ticket.Price;
            Event = Ticket.Event;
            Reference = GenerateUniqueReference();
            BookingFee = ticket.BookingFee;
            Payments = new List<Payment>();
        }

        public virtual string GenerateUniqueReference()
        {
            return "EV{0}/T{1}/C{2}/{3}".FormatString(Event.ID, Ticket.ID, Customer.ID.EncryptInteger(), DateTime.Now.ToString("MMddHHmmss"));
        }

        public virtual string Reference { get; set; }
        public virtual int Quantity { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual decimal TicketPrice { get; set; }
        public virtual decimal BookingFee { get; set; }
        public virtual decimal TotalPaid { get; set; }
        public virtual BookingStatus Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual IList<Payment> Payments { get; set; }

        public virtual decimal TicketsTotal
        {
            get { return Quantity * TicketPrice; }
        }
        public virtual decimal FeesTotal
        {
            get { return Quantity * BookingFee; }
        }

        public virtual decimal Total
        {
            get{ return TicketsTotal + FeesTotal; }
        }

        public virtual void UpdateQuantity(int qty)
        {
            Quantity = qty;
            if (Quantity < 1) Quantity = 1;
        }

        public virtual Payment AddPayment()
        {
            var p =new Payment(this, Total, Reference);
            Payments.Add(p);
            return p;
        }

        public virtual void FailPayment(Payment payment, string message, string details)
        {
            payment.Status = Payment.PaymentStatus.Failed;
            payment.Details = details;
            payment.Message += message + " <br />";
        }

        public virtual void MarkPaymentAsVerfied(Payment payment, string message, string details)
        {
            payment.Status = Payment.PaymentStatus.Verified;
            payment.Details = details;
            payment.Message += message + " <br />";
        }

        public virtual void MarkPaymentAsComplete(Payment payment, string details)
        {
            payment.Status = Payment.PaymentStatus.Complete;
            payment.Details = details;
            TotalPaid += payment.Amount;
            if (TotalPaid >= Total) Status = BookingStatus.Complete;
            Ticket.ReCalculateTicketsSold();
        }

        public virtual ReturnValue CanMakePayment()
        {
            if(Status == BookingStatus.Cancelled) return new ReturnValue(false, "Failed to proceed to payment. Your booking has been cancelled");
            if(Status == BookingStatus.Complete) return new ReturnValue(false, "Failed to proceed to payment. Your booking has already been completed");
            return new ReturnValue();
        }
        public virtual bool CanDelete()
        {
            if ((Status == BookingStatus.Reservation) && (TotalPaid == 0)) return true;
            return false;
        }
       

    }
}
