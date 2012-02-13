using System;
using System.Text;
using Bronson.DB.Common.Exceptions;
using RazorEngine;
using RazorEngine.Templating;

namespace Bronson.DB.Common.Model
{
    public class Query
    {
        public string QueryName { get; set; }
        public string CountQuery { get; set; }
        public string SelectQuery { get; set; }

        public void ParseSelectQuery(dynamic param)
        {
            if (param == null) return;
            try
            {
                SelectQuery = Razor.Parse(SelectQuery, param);
            }
            catch (TemplateCompilationException ex)
            {
                var message = BuildRazorParseErrorText(QueryName, ex);
                throw new RazorPaserException(message.ToString());
            }
        }
        
        private static StringBuilder BuildRazorParseErrorText(string queryName, TemplateCompilationException ex)
        {
            var message = new StringBuilder();
            foreach (var item in ex.Errors)
            {
                message.AppendLine(String.Format("{0} in {1} at ({2},{3})", item.ErrorText, queryName, item.Line, item.Column));
            }
            return message;
        }
    }
}
