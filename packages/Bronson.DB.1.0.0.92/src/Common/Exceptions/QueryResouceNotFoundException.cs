using System;
using System.Runtime.Serialization;

namespace Bronson.DB.Common.Exceptions
{
    [Serializable]
    public class QueryResouceNotFoundException : Exception
    {

        #region standard exection constructors
        public QueryResouceNotFoundException()
        {

        }

        public QueryResouceNotFoundException(string message) : base(message)
        {
            
        }

        public QueryResouceNotFoundException(string message, Exception innerException) :base(message,innerException)
        {
        
        }

        public QueryResouceNotFoundException(SerializationInfo info, StreamingContext context): base(info,context)
        {

        }
        #endregion

    }
}
