using Bronson.DB.Common;
using Bronson.DB.Common.Interface;
using NHibernate;
using StructureMap;
using Tarts.Persistance;

namespace Tarts.Web {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<ISession>().Use(y => DBHelper.NHibernateSessionFactory.GetCurrentSession());
                            x.For<IDBManager>().Use(y => DBHelper.BronsonDBManager);
                            x.For<ICacheService>().Use<DefaultCacheService>(); 
                           
                            
                        });
            return ObjectFactory.Container;
        }
    }
}
