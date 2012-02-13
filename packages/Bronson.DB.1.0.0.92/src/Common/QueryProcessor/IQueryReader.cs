
using Bronson.DB.Common.Model;

namespace Bronson.DB.Common.QueryProcessor
{
    public interface IQueryReader
    {
        Query GetQuery(string name);
    }
}
