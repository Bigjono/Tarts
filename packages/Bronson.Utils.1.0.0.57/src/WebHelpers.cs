using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Bronson.Utils
{
    public class WebHelpers
    {


        #region Paths

        /// <summary>
        /// Gets full server name, for example: http://mydomain.com
        /// </summary>
        public static string FullHostName()
        {
            Uri uri = HttpContext.Current.Request.Url;
            StringBuilder sb = new StringBuilder();
            sb.Append(uri.Scheme);
            sb.Append("://");
            sb.Append(uri.Host);
            return sb.ToString();
        }

        /// <summary>
        /// Gets full site root including slash at the end for example http://mydomain.com/app1/dir2/ or http://mydomain.com/
        /// </summary>
        public static string FullSiteRoot()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FullHostName());
            sb.Append(HttpContext.Current.Request.ApplicationPath);
            if (!HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
                sb.Append("/");
            string retVal = sb.ToString();
            return retVal;
        }

        /// <summary>
        /// For example ~/Abc/Def.aspx?abc=123&xyx=abc
        /// </summary>
        public static string CurrentPath()
        {

            string queryString = HttpContext.Current.Request.Url.Query;
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath + queryString;
        }

        /// <summary>
        /// For example ~/Abc/Def.aspx
        /// </summary>
        public static string RelativeToAppPath()
        {

            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;
        }

        /// <summary>
        /// Gets site path relative to domain root with slash at the end for example /dir1/ or /
        /// </summary>
        public static string PathToApp()
        {
            StringBuilder sb = new StringBuilder();
            if (!HttpContext.Current.Request.ApplicationPath.StartsWith("/"))
                sb.Append("/");
            sb.Append(HttpContext.Current.Request.ApplicationPath);
            if (!HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
                sb.Append("/");
            string retVal = sb.ToString();
            return retVal;
        }

        #endregion


        #region Files

        public static void DownloadFile(string fileName, string fileData)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName.Replace(" ", "_"));
            HttpContext.Current.Response.ContentType = fileName.GetMimeType();
            HttpContext.Current.Response.Write(fileData);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        public static void DownloadFile(string fileName, byte[] bytes)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = fileName.GetMimeType();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName.Replace(" ", "_"));
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        
        #endregion

        #region Forms

        public void RemotePost(string url, NameValueCollection formInputs)
        {
            RemotePost(url, formInputs, "post", "remotepost");
        }

        public void RemotePost(string url, NameValueCollection formInputs, string method, string formName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("<html><head>");
            HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", formName));
            HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", formName, method, url));

            for (int i = 0; i < formInputs.Keys.Count; i++)
                HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", formInputs.Keys[i], formInputs[formInputs.Keys[i]]));
            
            HttpContext.Current.Response.Write("</form>");
            HttpContext.Current.Response.Write("</body></html>");
            HttpContext.Current.Response.End();
        }

        #endregion  
    }
}
