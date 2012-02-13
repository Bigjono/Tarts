using System;
using System.Collections.Generic;
using System.Data;

namespace Bronson.DB.Common.ContextHandler
{
    public static class StaticContextFactory
    {

        static StaticContextFactory()
        {
            Conenctions = new Dictionary<string, IDbConnection>();
        }


        [ThreadStatic] private static readonly IDictionary<string, IDbConnection> Conenctions;

        public static void Set(string key, IDbConnection currentConnection)
        {
            try
            {
                Conenctions.Add(key, currentConnection);
            }
            catch 
            {  }
            

        }

        public static void Clear(string key)
        {
            try
            {
                Conenctions[key] = null;
            }
            catch 
            {
                
                
            }
            
        }

        public static IDbConnection CurrentConnection(string key)
        {
            try
            {
                return Conenctions[key];
            }
            catch  
            {

                return null;
            }
            
        }
    }
}
