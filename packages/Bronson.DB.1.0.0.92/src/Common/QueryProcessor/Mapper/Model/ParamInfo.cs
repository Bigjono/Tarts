using System.Data;

namespace Bronson.DB.Common.QueryProcessor.Mapper.Model
{
    
     public class ParamInfo
    {
        public static ParamInfo Create(string name, DbType type, object val)
        {
            return new ParamInfo { Name = name, Type = type, Val = val };
        }

        public DbType Type { get; private set; }
        public string Name { get; private set; }
        public object Val { get; private set; }



    }
}
