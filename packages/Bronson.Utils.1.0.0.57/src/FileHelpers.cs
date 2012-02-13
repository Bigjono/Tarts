using System;
using System.IO;
using System.Xml.Serialization;

namespace Bronson.Utils
{
    public class FileHelpers
    {
        public static string CreateUniqueFilename(string sExtension)
        {
            string retVal = DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss_") + Guid.NewGuid().ToString();
            if (sExtension.Length > 0)
            {
                if (sExtension.StartsWith("."))
                    retVal += sExtension;
                else
                    retVal += "." + sExtension;
            }
            return retVal;
        }

        public static byte[] ReadFile(string fullPath)
        {
            try { return System.IO.File.ReadAllBytes(fullPath); }
            catch (Exception) {   return null; }

        }


        public static bool DeleteFile(string fullPath)
        {
            try
            {
                System.IO.File.Delete(fullPath);
                return true;
            }
            catch (Exception) { return false; }

        }

        public static string SerializeToString<T>(T obj)
        {
            StringWriter sw = new StringWriter();

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(sw, obj);

            return sw.ToString();
        }
    }
}
