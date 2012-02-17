using FluentNHibernate.Mapping;
using Tarts.Customers;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class CustomerMap : ClassMap<Customer>
    {

        public CustomerMap()
        {
            Table("Customer");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.FirstName);
            Map(x => x.Surname);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.Mobile);
            Map(x => x.Subscribed);
            Map(x => x.FacebookID);
            

           

        }


    }
}
