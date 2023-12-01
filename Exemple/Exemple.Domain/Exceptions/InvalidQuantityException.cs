using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Exemple.Domain.Exceptions
{
    [Serializable]
    internal class InvalidQuantityException : Exception
    {
        public InvalidQuantityException()
        {
        }

        public InvalidQuantityException(string message) : base(message)
        {
        }

        public InvalidQuantityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidQuantityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
