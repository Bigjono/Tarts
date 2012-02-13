using System;
using System.Runtime.Serialization;

namespace Bronson.DB.Common.Exceptions
{
    [Serializable]
    public class RazorPaserException: Exception
    {

         #region standard exection constructors
        public RazorPaserException()
        {

        }

        public RazorPaserException(string message) : base(message)
        {
            
        }

        public RazorPaserException(string message, Exception innerException) :base(message,innerException)
        {
        
        }

        public RazorPaserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        #endregion
    }
}
