using System.Text.RegularExpressions;
using Bronson.Utils;

namespace System
{
    public static class Strings
    {

        public static string ProcessVelocityTemplate(this string s, object model)
        {
            return Templating.ProcessVelocityTemplate(s, model);
        }

        public static string ToCamelCase(this string s)
        {
            s = s.ToLower();
            string sProper = "";
            foreach (var ss in s.Split(' '))
            {
                sProper += char.ToUpper(ss[0]);
                sProper += (ss.Substring(1, ss.Length - 1) + ' ');
            }

            return sProper.Replace(" ", "");
        }
        public static string FormatString(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static string SplitCamelCase(this string s)
        {
            return Regex.Replace(Regex.Replace(s, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string RemoveNameSpaces(this string typeName)
        {
            int lastDotIndex = typeName.LastIndexOf(".") + 1;
            return typeName.Substring(lastDotIndex, typeName.Length - lastDotIndex);
        }

        public static string RemoveFirstNameSpace(this string typeName)
        {
            int dotIndex = typeName.IndexOf(".") + 1;
            return typeName.Substring(dotIndex, typeName.Length - dotIndex);
        }

        public static string Pluralize(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            
            if (!text.ToLower().EndsWith("s"))
                text += "'s";
            return text;
            
        }

        /// <summary>
        /// Trims string at exact length, ending in ...
        /// </summary>
        public static string Trim(this string value, int length)
        {
            return Trim(value, length, "..."); 
        }

        /// <summary>
        /// Trims string at exact length, ending in ...
        /// </summary>
        public static string Trim(this string value, int length, string postfix)
        {
            if (value == null) return value;
            if (length >= value.Length)
                return value;
            string contentCut = value.Substring(0, length - postfix.Length);
            return contentCut += postfix;
        }

        /// <summary>
        /// Trims string within given length to the nearest whole word followed by ...
        /// </summary>
        public static string SmartTrim(this string value, int length)
        {
            return SmartTrim(value, length, "..."); 
        }

        /// <summary>
        /// Trims string within given length to the nearest whole word
        /// </summary>
        public static string SmartTrim(this string value, int length, string postfix)
        {
            if (value == null) return value;
            if (length >= value.Length)
                return value;
            string contentCut = value.Substring(0, length - postfix.Length);

            int lastSpaceIndex = contentCut.LastIndexOf(" ");
            if (lastSpaceIndex != -1)
                contentCut = value.Substring(0, lastSpaceIndex);

            return contentCut += postfix;
        }

        /// <summary>
        /// Compares to strings regardless of case or end spaces
        /// </summary>
        public static bool Match(this string obj, string value)
        {
            if ((string.IsNullOrEmpty(obj)) || (string.IsNullOrEmpty(value)))
                return (string.IsNullOrEmpty(obj) && string.IsNullOrEmpty(value));

            return obj.Trim().ToLower() == value.Trim().ToLower();
        }
    }
}
