using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Bronson.DB.Common;
using Bronson.DB.Common.Interface;
using Bronson.DB.Config;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using StructureMap;
using Tarts.Persistance.Mapping;
using Configuration = Bronson.DB.Config.Configuration;

namespace Tarts.Persistance
{
    public static class DBHelper
    {

        static DBHelper()
        {
            NHHibernateSessionFactory = CreateWatchfinderSessionFactory();
            Abort = false;
            BronsonDBManager = GetBronsonDBConnection();
        }

        public static IDBManager BronsonDBManager { get; private set; }
        public static ISessionFactory NHHibernateSessionFactory;
        public static ITransaction NHHibernateTransaction;
        public static bool Abort { get; set; }

        public static ISessionFactory CreateWatchfinderSessionFactory()
        {

            return Fluently.Configure()
                           .Database(MySQLConfiguration.Standard.ConnectionString(c => c.FromConnectionStringWithKey("tartsdataconnection")))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PageMap>())
                           .ExposeConfiguration(c =>
                           {
                               c.SetProperty("generate_statistics", "true");
                               c.SetProperty("show_sql", "false");
                               c.SetProperty("adonet.batch_size", "500");
                               c.SetProperty("connection.release_mode", "auto");
                               c.SetProperty("current_session_context_class", "web");
                               c.SetProperty("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                               c.SetProperty("hbm2ddl.keywords", "none");
                           })
                       .BuildSessionFactory();

        }

        public static ISessionFactory NHibernateSessionFactory
        {
            get { return DBHelper.NHHibernateSessionFactory; }
        }


        public static void Commit()
        {
            NHHibernateTransaction.Commit();
            NHibernateSessionFactory.GetCurrentSession().Flush();
        }

        public static void Rollback()
        {
            NHHibernateTransaction.Rollback();
        }

        public static void BeginTransaction()
        {
            NHHibernateTransaction = NHibernateSessionFactory.GetCurrentSession().BeginTransaction();
        }

        private static IDBManager GetBronsonDBConnection()
        {

            var dbConfig = Bronson.DB.Config.Configure.With()
                    .WithConnectionSettingsFromConfig("tartsdataconnection")
                    .AddQueryParserRule(@"\[tartsdb\]", ConfigurationManager.AppSettings["tartsdb"])
                    .WithContextModeOf(Configuration.CurrentContextModes.Web)
                    .OverrideCacheServiceWith(ObjectFactory.GetInstance<ICacheService>());

            var dbManager = new DBManager();
            dbManager.ConfigureDBManager(dbConfig);
            return dbManager;
        }


    }
}
