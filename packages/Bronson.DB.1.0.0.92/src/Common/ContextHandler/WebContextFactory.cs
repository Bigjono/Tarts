using System;
using System.Collections;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Bronson.DB.Common.ContextHandler
{
    public class WebContextFactory
    {
        


        static WebContextFactory()
        {
            CreateCurrentHttpContextGetter();
            CreateHttpContextItemsGetter();
        }


        public static Func<object> HttpContextCurrentGetter { get; private set; }

        public static Func<object, IDictionary> HttpContextItemsGetter { get; private set; }

        public static IDictionary HttpContextCurrentItems
        {
            get { return HttpContextItemsGetter(HttpContextCurrentGetter()); }
        }

        private static Type HttpContextType
        {
            get
            {
                return
                    Type.GetType(
                        string.Format(
                            "System.Web.HttpContext, System.Web, Version={0}, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
                                Environment.Version));
            }
        }

        private static void CreateCurrentHttpContextGetter()
        {
            PropertyInfo currentProperty = HttpContextType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            Expression propertyExpression = Expression.Property(null, currentProperty);
            Expression convertedExpression = Expression.Convert(propertyExpression, typeof(object));
            HttpContextCurrentGetter = (Func<object>)Expression.Lambda(convertedExpression).Compile();

        }

        private static void CreateHttpContextItemsGetter()
        {
            ParameterExpression contextParam = Expression.Parameter(typeof(object), "context");
            Expression convertedParam = Expression.Convert(contextParam, HttpContextType);
            Expression itemsProperty = Expression.Property(convertedParam, "Items");
            HttpContextItemsGetter = (Func<object, IDictionary>)Expression.Lambda(itemsProperty, contextParam).Compile();
        }

        public static void Set(string key,IDbConnection currentConnection)
        {

            HttpContextCurrentItems.Add(key, currentConnection);
        }

        public static void Clear(string key)
        {

            try
            {
                HttpContextCurrentItems.Remove(key);
            }
            catch 
            {  }
            
            
        }

        public static IDbConnection CurrentConnection(string key)
        {
            try
            {
                return HttpContextCurrentItems[key] as IDbConnection;
            }
            catch
            {
                return null;
            }
        }
    }
}
