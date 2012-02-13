using System;
using System.Linq;
using System.Xml.Linq;
using Bronson.DB.Common.Exceptions;
using Bronson.DB.Common.Model;
using Bronson.DB.Common.Resouce;

namespace Bronson.DB.Common.QueryProcessor
{
    public class XMLQueryReader : IQueryReader
    {

        private readonly ResouceFileReader _resouceFileReader;
        private readonly Type _requestedType;

        public XMLQueryReader(Type forType, string defaultExtension = "xml")
        {
            _requestedType = forType;
            _resouceFileReader = new ResouceFileReader(forType, defaultExtension);
        }

        public Query GetQuery(string name)
        {
            var queryText = _resouceFileReader.GetStringFromResouceFile(_requestedType.ToString());
            return ExtractQueries(name, queryText);
        }

        private Query ExtractQueries(string queryName, string queryText)
        {

            if (string.IsNullOrEmpty(queryText))
            {
                ThrowQueryNotFoundError(queryName);
            }

            XElement root = XElement.Parse(queryText);
            XElement query = root.Elements("query").Where(x => x.Attribute("name").Value == queryName).FirstOrDefault();

            if (query == null)
            {
                ThrowQueryNotFoundError(queryName);
            }
            var selectQuery = GetValueFromElement(query, "selectquery");
            var countQuery = GetValueFromElement(query, "countquery");

            return new Query() { SelectQuery = selectQuery, CountQuery = countQuery };

        }

        private static string GetValueFromElement(XElement query,string elementName)
        {
            try
            {
                return  query.Element(elementName).Value;
            }
            catch (Exception)
            {
                return "";
            }

        }

        private static void ThrowQueryNotFoundError(string queryName)
        {
            throw new QueryResouceNotFoundException(string.Format("{0} - Not Found,  have you set it to be an embedded resource.", queryName));
        }
    }

}
