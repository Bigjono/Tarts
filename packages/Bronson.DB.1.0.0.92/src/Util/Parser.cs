using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bronson.DB.Util
{
    public class Parser
    {

        #region dependacy
        private readonly ConcurrentDictionary<string, string> _rulesCollection;
        #endregion

        #region constructor
        public Parser(ConcurrentDictionary<string, string> queryRulesCollection)
        {
            _rulesCollection = queryRulesCollection ?? new ConcurrentDictionary<string, string>();
        }
        #endregion


        public string ParseString(string sql)
        {
            if (NoRulesToParse()) return sql;
            return  _rulesCollection.Aggregate(sql, (current, item) => ParseItem(current, item.Key, item.Value));
        }

        private bool NoRulesToParse()
        {
            if ( _rulesCollection == null) return true;
            if (_rulesCollection.ToList().Count == 0) return true;
            return false;
        }

        #region helpers
        private string ParseItem(string sql, string lookFor, string replaceWith)
        {
            return Regex.Replace(sql, lookFor, replaceWith, RegexOptions.IgnoreCase);
        }
        #endregion

    }
}
