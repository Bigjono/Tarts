using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tarts.Web.Areas.TartsAdmin.Models.Images;

namespace Tarts.Web.Areas.TartsAdmin.Controllers.Base
{
    public abstract class MultipleFileUploadBaseController : Controller
    {
        public abstract List<FilesStatus> GetFiles(HttpRequestBase request);

        public abstract FilesStatus SaveFile(HttpRequestBase request, HttpPostedFileBase file);

        public abstract FilesStatus SavePartialFile(HttpRequestBase request, Stream inputStream, string fileName, long totalFileSize);

        public ActionResult HandleInput()
        {
            var context = HttpContext;

            List<FilesStatus> statuses = new List<FilesStatus>();

            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    statuses = HandleFileServe(context);
                    break;
                case "POST":
                    statuses = HandleFileUpload(context);
                    break;
                default:
                    HttpContext.Response.ClearHeaders();
                    HttpContext.Response.StatusCode = 405;
                    break;
            }
            WriteFileDetailToResponse(context, statuses);
            return null;
        }

        private List<FilesStatus> HandleFileServe(HttpContextBase context)
        {
            return HandleMultiFiles(context.Request);
        }

        private List<FilesStatus> HandleFileUpload(HttpContextBase context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                if (context.Request.Files.Count > 0)
                    statuses.Add(SaveFile(context.Request, context.Request.Files[0]));
            }
            else
            {
                if (context.Request.Files.Count > 0)
                {
                    var stream = context.Request.Files[0].InputStream;
                    statuses.Add(SavePartialFile(context.Request, stream, headers["X-File-Name"],
                                                 long.Parse(headers["X-File-Size"])));
                }
            }


            return statuses;
        }

        private List<FilesStatus> HandleMultiFiles(HttpRequestBase request)
        {
            return GetFiles(request);
        }

        private void WriteFileDetailToResponse(HttpContextBase context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                {
                    context.Response.ContentType = "application/json";
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                }
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var js = new JavaScriptSerializer();

            var jsonObj = js.Serialize(statuses.ToArray());
            context.Response.Write(jsonObj);
        }


    }

    
}
