using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Areas.TartsAdmin.Models.Images
{
    public class MultipleFileUploadConfig
    {
        public MultipleFileUploadConfig(string uploadContext = "", string fileUploaderID = "fileupload")
        {
            FileUploaderID = fileUploaderID;
            UploadContext = uploadContext;
            AutoUpload = false;
            MaxNumberOfFiles = -1;
            MaxFileSize = -1;
            MinFileSize = -1;
        }

        public string UploadContext { get; private set; }
        public string FileUploaderID { get; private set; }
        public bool AutoUpload { get; set; }
        public int MaxNumberOfFiles { get; set; }
        public int MaxFileSize { get; set; }
        public int MinFileSize { get; set; }

        /* look at jquery.fileupload-ui.js for more configs */
    }
}