using Tarts.Base;

namespace Tarts.Bookings
{
    public class Voucher : EntityBase
    {
        public enum DiscountApplications : int
        {
            BookingTotal = 1,
            PerTicket = 2
        }

        public virtual string Code { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual DiscountApplications DiscountApplication { get; set; }


        public Voucher()
        {
            Code = Bronson.Utils.Random.RandomString(6);
            Discount = 10;
            DiscountApplication = DiscountApplications.PerTicket;
        }

        
       
    }
}
