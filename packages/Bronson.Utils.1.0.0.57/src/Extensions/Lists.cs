using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class Lists
    {

        public static string ToJsArray(this IList<string> lst)
        {
            string result1 = lst.Aggregate("[", (current, s) => current + ((current != "[") ? "," : "") + "'{0}'".FormatString(s.Replace("'","")));
            string result = result1 + "]";
            return result;
        }
    }
}
