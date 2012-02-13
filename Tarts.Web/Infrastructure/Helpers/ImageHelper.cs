using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ImageResizer;
using ImageResizer.Util;
using Tarts.Assets;
using Tarts.Persistance;

namespace Tarts.Web.Infrastructure.Helpers
{
    public class ImageHelper
    {

        private GenericRepo Repo;
        public ImageHelper()
        {
            Repo = new GenericRepo(DBHelper.NHibernateSessionFactory.GetCurrentSession());
        }


        public Image SaveImage(HttpServerUtilityBase server, HttpPostedFileBase file)
        {

            string largeUploadFolder = server.MapPath("~/assets/images/large");
            string mediumUploadFolder = server.MapPath("~/assets/images/medium");
            string thumbUploadFolder = server.MapPath("~/assets/images/thumb");
            if (!Directory.Exists(largeUploadFolder)) Directory.CreateDirectory(largeUploadFolder);
            if (!Directory.Exists(mediumUploadFolder)) Directory.CreateDirectory(mediumUploadFolder);
            if (!Directory.Exists(thumbUploadFolder)) Directory.CreateDirectory(thumbUploadFolder);

            //The resizing settings can specify any of 30 commands.. See http://imageresizing.net for details.
            ResizeSettings largeSettings = new ResizeSettings("maxwidth=800&maxheight=800");
            ResizeSettings mediumSettings = new ResizeSettings("maxwidth=300&maxheight=300&scale=both");
            ResizeSettings thumbSettings = new ResizeSettings("width=100&height=100&crop=auto");

            //var uniqueName = System.Guid.NewGuid().ToString();
            string uniqueName = PathUtils.RemoveExtension(file.FileName) + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            string largeFilePath = Path.Combine(largeUploadFolder, uniqueName);
            string mediumFilePath = Path.Combine(mediumUploadFolder, uniqueName);
            string thumbFilePath = Path.Combine(thumbUploadFolder, uniqueName);

            //Let the image builder add the correct extension based on the output file type (which may differ).
            var large = ImageBuilder.Current.Build(file, largeFilePath, largeSettings, false, true);
            var med = ImageBuilder.Current.Build(file, mediumFilePath, mediumSettings, false, true);
            var thumb = ImageBuilder.Current.Build(file, thumbFilePath, thumbSettings, false, true);


            Image img = new Image(PathUtils.RemoveExtension(file.FileName), ResolveRelativePath(server, large), ResolveRelativePath(server, med), ResolveRelativePath(server, thumb));
            Repo.Save(img);
            return img;
        }

        public void Delete(HttpServerUtilityBase server, Image img)
        {
            
            try { System.IO.File.Delete(server.MapPath(img.Large)); } catch {}
            try { System.IO.File.Delete(server.MapPath(img.Medium)); } catch {}
            try { System.IO.File.Delete(server.MapPath(img.Thumb)); } catch {}
            Repo.Delete(img);
            
        }

        private static string ResolveRelativePath(HttpServerUtilityBase server, string physicalPath)
        {
            //C:\@Projects\Tarts\Tarts.Web\assets\images\large\1040c396-c558-473e-a3ca-f20de6ab13d2.jpg
            string relative = "~/assets/images";
            string physical = server.MapPath(relative);
            return physicalPath.Replace(physical, relative).Replace("\\", "/").Replace("~","");
        }

    }
}