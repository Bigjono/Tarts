using FluentNHibernate.Mapping;
using Tarts.Admin;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class UserMap : ClassMap<User>
    {

        public UserMap()
        {
            Table("User");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.FirstName);
            Map(x => x.Surname);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.Enabled);

        }


    }
}
