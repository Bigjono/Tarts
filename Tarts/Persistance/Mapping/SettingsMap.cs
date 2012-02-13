using FluentNHibernate.Mapping;
using Tarts.Config;
using Tarts.Events;

namespace Tarts.Persistance.Mapping
{
    public class SettingsMap : ClassMap<Settings>
    {

        public SettingsMap()
        {
            Table("Settings");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.AnalyticsTrackingCode);
            Map(x => x.SmtpHost);
            Map(x => x.SmtpUsername);
            Map(x => x.SmtpPassword);
            Map(x => x.ForceEmailsTo);
            Map(x => x.EmailFromName);
            Map(x => x.EmailFromAddress);
            Map(x => x.PaypalUrl);
            Map(x => x.PaypalPdtToken);
            Map(x => x.PaypalUsername);

        }


    }
}
