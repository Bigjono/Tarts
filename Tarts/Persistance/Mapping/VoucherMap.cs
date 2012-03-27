using FluentNHibernate.Mapping;
using Tarts.Bookings;
using Tarts.Customers;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class VoucherMap : ClassMap<Voucher>
    {

        public VoucherMap()
        {
            Table("Voucher");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Code);
            Map(x => x.Discount);
            Map(x => x.Enabled);
            Map(x => x.DiscountApplication).CustomType(typeof(Voucher.DiscountApplications)).Default((Voucher.DiscountApplications.BookingTotal.ToString()));

        }


    }
}
