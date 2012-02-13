using System;
using System.IO;
using System.Linq;
using Bronson.DB.Common.Exceptions;

namespace Bronson.DB.Common.Resouce
{
    public class ResouceFileReader
    {

        private string _defaultFileExtension;
        private readonly Type _requestedType;

        #region constructor
        public ResouceFileReader(Type forType,string defaultExtension)
        {
            _requestedType = forType;
            RegisterFileExtension(defaultExtension);  // sets the default query type extension to be .sql.
        }
        #endregion


        #region public method
        public string GetStringFromResouceFile(string fileName)
        {

            return QueryCacheController.Fetch(AppendFileExtension(fileName), LoadContentsFromResouce);
        }

        #endregion

        #region private helpers

        private string LoadContentsFromResouce(string fileName)
        {

            var resourceNames = _requestedType.Assembly.GetManifestResourceNames();

            foreach (var resourceName in resourceNames.Where(resourceName => resourceName.ToLower().EndsWith(fileName.ToLower())))
            {
                return ResouceStreamToString(resourceName);
            }

            // throw an resource not found error
            throw new QueryResouceNotFoundException(string.Format("{0} - Not Found,  have you set it to be an embedded resource.", fileName));


        }

        private string ResouceStreamToString(string resourceName)
        {

            using (var stream = _requestedType.Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return "";
                using (var reader = new StreamReader(stream)) { return reader.ReadToEnd(); }
            }
        }

        private string AppendFileExtension(string queryName)
        {
            if (!queryName.ToLower().EndsWith(_defaultFileExtension))
            {
                queryName += _defaultFileExtension;
            }
            return queryName;
        }

        private void RegisterFileExtension(string extension)
        {
            _defaultFileExtension = !extension.StartsWith(".") ? "." : "";
            _defaultFileExtension += extension;
        }

        #endregion
    }
}
