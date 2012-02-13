using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Tarts.Content;

namespace Tarts.Persistance.Mapping
{
    public class PageMap : ClassMap<Page>
    {

        public PageMap()
        {
            Table("Page");
            Id(x => x.ID).GeneratedBy.Identity().UnsavedValue(0);
            Map(x => x.Name);
            Map(x => x.Content);

        }


    }
}
