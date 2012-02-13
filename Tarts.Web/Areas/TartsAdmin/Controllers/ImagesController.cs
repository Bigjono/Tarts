using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageResizer;
using Tarts.Assets;
using Tarts.Content;
using Tarts.Persistance;
using Tarts.Web.Areas.TartsAdmin.Controllers.Base;
using Tarts.Web.Areas.TartsAdmin.Models.Images;
using Tarts.Web.Infrastructure.Helpers;

namespace Tarts.Web.Areas.TartsAdmin.Controllers
{
    public class ImagesController : MultipleFileUploadBaseController
    {
        private GenericRepo Repo;
        private ImageHelper ImageHelper;
        public ImagesController(GenericRepo repo, ImageHelper imageHelper)
        {
            Repo = repo;
            ImageHelper = imageHelper;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View(Repo.GetAll<Image>());
        }
        [Authorize]
        public ActionResult EditorPlugin()
        {
            return View(Repo.GetAll<Image>());
        }
        [Authorize]
        public ActionResult ImageSelector()
        {
            return View(Repo.GetAll<Image>());
        }
        [Authorize]
        public ActionResult Update(int ID, string name)
        {
            var obj = (ID == 0) ? new Image() : Repo.GetById<Image>(ID);
            if(obj != null)
            {
                obj.Name = name;
                Repo.Save(obj);
            }
            TempData["Message"] = "Image Saved";
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult Destroy(int id)
        {
            var img = Repo.GetById<Image>(id);
            ImageHelper.Delete(Server,img);
            TempData["Message"] = "Image Deleted";
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult SilentDestroy(int id)
        {
            var img = Repo.GetById<Image>(id);
            ImageHelper.Delete(Server, img);
            return null;
        }
        [Authorize]
        public ActionResult FileUpload(string uploadContext)
        {
            return View(new MultipleFileUploadConfig(uploadContext));
        }



        #region Multiple File Upload


        public override List<FilesStatus> GetFiles(HttpRequestBase req)
        {
            var statuses = new List<FilesStatus>();

            //int productId = req.QueryString["ProductID"].ConvertToInt32(0);
            //if (productId == 0) return statuses;

            //var images = _imageQueries.GetImagesForProduct(productId);

            //foreach (var image in images)
            //{
            //    statuses.Add(ConvertImageToFileStatus(image));
            //}

            return statuses;
        }

        public override FilesStatus SaveFile(HttpRequestBase req, HttpPostedFileBase file)
        {
            var status = new FilesStatus();

            var img = ImageHelper.SaveImage(Server, file);

            AddUploadedImageToDomainObject(img);

            return new FilesStatus()
            {
                name = file.FileName,
                alt_text = img.Name,
                size = file.ContentLength,
                type = "image/jpeg",
                thumbnail_url = img.Thumb,
                delete_url = "/images/SilentDestroy/" + img.ID.ToString(),
                update_url = "/images/update/" + img.ID.ToString(),
                update_type = "POST",
                delete_type = "POST",
            };

            
        }

        public override FilesStatus SavePartialFile(HttpRequestBase req, Stream inputStream, string fileName, long totalFileSize)
        {
            //int productId = req.QueryString["ProductID"].ConvertToInt32(0); if (productId == 0) return new FilesStatus();

            //var fullPath = string.Format(@"C:\tmp\{0}", fileName);

            //using (var fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write))
            //{
            //    var buffer = new byte[1024];

            //    var l = inputStream.Read(buffer, 0, 1024);
            //    while (l > 0)
            //    {
            //        fs.Write(buffer, 0, l);
            //        l = inputStream.Read(buffer, 0, 1024);
            //    }

            //    if (fs.Length >= totalFileSize) //download finished
            //    {
            //        ReturnValue<string> fileSave = _fileService.SaveImageAndCreateFilePath(fullPath, GetSubFolderPath(), System.IO.Path.GetFileNameWithoutExtension(fileName));

            //        if (fileSave.Succeeded)
            //        {
            //            var result = CreateProductImageFromFile(fileName, fullPath, fileSave, productId);

            //            if (result.Success)
            //            {
            //                var image = _imageQueries.GetByProductIDAndFileName(productId, fileName);
            //                return ConvertImageToFileStatus(image);
            //            }
            //        }
            //    }
            //}


            //return new FilesStatus
            //{
            //    thumbnail_url = string.Empty,
            //    url = string.Empty,
            //    name = fileName,
            //    size = (int)(new FileInfo(fullPath)).Length,
            //    type = "image/jpeg",
            //    delete_url = string.Empty,
            //    delete_type = "DELETE",
            //    progress = "1.0"
            //};
            return new FilesStatus();
        }

        private void AddUploadedImageToDomainObject(Image img)
        {
            string ctx = Request.QueryString["uploadContext"];
            if(ctx.StartsWith("gallery_"))
            {
                int galleryID = System.Convert.ToInt32(Request["uploadContext"].Replace("gallery_",""));
                var gallery = Repo.GetById<Gallery>(galleryID);
                if (gallery != null)
                {
                    gallery.AddPhoto(img);
                    Repo.Save(gallery);
                }
            }
        }


        #endregion

    }
}
