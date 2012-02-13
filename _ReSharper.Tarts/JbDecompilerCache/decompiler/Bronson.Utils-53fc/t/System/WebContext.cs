// Type: System.WebContext
// Assembly: Bronson.Utils, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\@Projects\Tarts\packages\Bronson.Utils.1.0.0.57\lib\net40\Bronson.Utils.dll

using Bronson.Utils;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
  public static class WebContext
  {
    public static string StripHtml(this string value)
    {
      return Regex.Replace(value, "<(.|\\n)*?>", string.Empty);
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

    public static string ToFullSitePath(this string url)
    {
      string newValue = WebHelpers.FullSiteRoot();
      return !url.StartsWith("~/") || !newValue.EndsWith("/") ? url.Replace("~", newValue) : url.Replace("~/", newValue);
    }

    public static string RemoveSiteRoot(this string url)
    {
      url = WebContext.ToFullSitePath(url);
      url = url.Replace(WebHelpers.FullSiteRoot(), string.Empty);
      if (url.IndexOf("/") == 0)
        url = url.Substring(1, url.Length - 1);
      return url;
    }

    public static string ToDomainRelativePath(this string url)
    {
      string str = WebContext.ToFullSitePath(url).Replace(WebHelpers.FullHostName(), "");
      if (!str.StartsWith("/"))
        str = "/" + str;
      return str;
    }

    public static string AddQueryStringToUrl(this string url, string queryString)
    {
      string input = url;
      if (queryString.Trim().Length > 0)
      {
        if (queryString.StartsWith("?") || queryString.StartsWith("&"))
          queryString = queryString.Substring(1, queryString.Length - 1);
        if (url.Contains("?"))
        {
          foreach (Match match in Regex.Matches(queryString, "(?<paramname>[^=&]*)=(?<paramvalue>[^&]*)"))
          {
            string pattern = (string) (object) match.Groups["paramname"] + (object) "=(?<paramvalue>[^&]*)";
            if (Regex.Match(url, pattern).Success)
              input = Regex.Replace(input, match.Groups["paramname"].Value + "=[^&]*", match.Groups["paramname"].Value + "=" + match.Groups["paramvalue"].Value);
            else
              input = input + "&" + match.Groups["paramname"].Value + "=" + match.Groups["paramvalue"].Value;
          }
        }
        else
          input = url + "?" + queryString;
      }
      return input;
    }

    public static void Redirect(this string url)
    {
      if (HttpContext.Current.Response.IsRequestBeingRedirected)
        return;
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
      else
        return HttpContext.Current.Session[key];
    }
  }
}
