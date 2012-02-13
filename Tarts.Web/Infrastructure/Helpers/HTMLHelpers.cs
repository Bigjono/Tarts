using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Tarts.Web.Infrastructure.Helpers
{
    public static class HTMLHelpers
    {

        public static MvcHtmlString Gravatar(this HtmlHelper htmlHelper, string email, int width)
        {
            var md5 = email.Trim().ToLower().Hash();
            var url = "http://www.gravatar.com/avatar/{0}?s={1}".FormatString(GenerateGravatarMD5(email),width);
            return new MvcHtmlString("<img src='{0}' title='gravatar'/>".FormatString(url));
        }


        private static string GenerateGravatarMD5(string originalPassword)
        {
          //Declarations
          Byte[] originalBytes;
          Byte[] encodedBytes;
          MD5 md5;

          //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
          md5 = new MD5CryptoServiceProvider();
          originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
          encodedBytes = md5.ComputeHash(originalBytes);

          //Convert encoded bytes back to a 'readable' string
          return BitConverter.ToString(encodedBytes).Replace("-","").ToLower();
        }
    }
}