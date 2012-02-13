using System.IO;
using System.Web;

namespace System
{
    public static class Files
    {

        /// <summary>
        /// Defaults to "application/unknown" if not found
        /// </summary>
        public static string GetMimeType(this string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public static string Extension(this HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName);
        }

        public static string Extension(this string fileName)
        {
            return Path.GetExtension(fileName);
        }

    }
}
