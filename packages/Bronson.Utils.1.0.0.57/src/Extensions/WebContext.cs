using System.Text.RegularExpressions;
using System.Web;
using Bronson.Utils;

namespace System
{
    public static class WebContext
    {


        public static string StripHtml(this string value)
        {
            return Regex.Replace(value, @"<(.|\n)*?>", string.Empty);
        }

        public static string HtmlEncode(this string value)
        {
            return HttpContext.Current.Server.HtmlEncode(value);
        }
        
        public static string HtmlDecode(this string value)
        {
            return HttpContext.Current.Server.HtmlDecode(value);
        }
        
        public static string UrlPathEncode(this string value)
        {
            return HttpContext.Current.Server.UrlPathEncode(value);
        }
        
        public static string UrlDecode(this string value)
        {
            return HttpContext.Current.Server.UrlDecode(value);
        }



        /// <summary>
        /// Gives full path to url, replaces ~/Test1/test2.html to http://server/myapp/Test1/test2.html
        /// </summary>
        public static string ToFullSitePath(this string url)
        {
            string sitePath = WebHelpers.FullSiteRoot();
            if (url.StartsWith("~/") && sitePath.EndsWith("/"))
                sitePath = url.Replace("~/", sitePath);
            else
                sitePath = url.Replace("~", sitePath);
            return sitePath;

        }

        /// <summary>
        /// Converts http://www.mydomain.com/home/news/latest.html to home/news/latest.html
        /// </summary>
        public static string RemoveSiteRoot(this string url)
        {
            url = ToFullSitePath(url);//convert url to full path
            url = url.Replace(WebHelpers.FullSiteRoot(), string.Empty);
            if (url.IndexOf("/") == 0)
                url = url.Substring(1, url.Length - 1);
            return url;
        }

        /// <summary>
        /// Gives full path to url, replaces ~/Test1/test2.html to /myapp/Test1/test2.html
        /// </summary>
        public static string ToDomainRelativePath(this string url)
        {
            string fullUrl = ToFullSitePath(url);
            string rv = fullUrl.Replace(WebHelpers.FullHostName(), "");
            if (!rv.StartsWith("/"))
                rv = "/" + rv;
            return rv;
        }
        
        public static string AddQueryStringToUrl(this string url, string queryString)
        {
            string retVal = url;
            if (queryString.Trim().Length > 0)
            {
                if ((queryString.StartsWith("?")) || (queryString.StartsWith("&")))
                    queryString = queryString.Substring(1, queryString.Length - 1);
                if (url.Contains("?"))
                {
                    MatchCollection matchCol = Regex.Matches(queryString, "(?<paramname>[^=&]*)=(?<paramvalue>[^&]*)");// there are already params
                    foreach (Match m in matchCol) // for each match in "query to add"
                    {
                        string regExStr = m.Groups["paramname"] + "=(?<paramvalue>[^&]*)";
                        Match matchInUrl = Regex.Match(url, regExStr);
                        if (matchInUrl.Success == true)
                            retVal = Regex.Replace(retVal, m.Groups["paramname"].Value + "=[^&]*", m.Groups["paramname"].Value + "=" + m.Groups["paramvalue"].Value); // already has this param, replace it
                        else
                            retVal += "&" + m.Groups["paramname"].Value + "=" + m.Groups["paramvalue"].Value;// doesn't have this param yet, add it
                    }
                }
                else
                    retVal = url + "?" + queryString; // simple append
            }
            return retVal;
        }

        public static void Redirect(this string url)
        {
            if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                HttpContext.Current.Response.Redirect(url, true);
        }
        
        public static void AddToSession(this string key, object value)
        {
            if (HttpContext.Current.Session[key] == null)
                HttpContext.Current.Session.Add(key, value);
            else
                HttpContext.Current.Session[key] = value;
        }
    
        public static object GetFromSession(this string key, object value)
        {
            if (HttpContext.Current.Session[key] == null)
                return value;
            return HttpContext.Current.Session[key];
        }
    }
}
