using System;
using System.Reflection;
using System.Web;

namespace Tarts.Web.Infrastructure.Helpers
{
    public static class ApplicationHelper
    {
        #region static constructor

        static ApplicationHelper()
        {
            VersionNumber = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        #endregion


        #region props

        public static string VersionNumber { get; private set; }



        #endregion
    }


}