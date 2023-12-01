﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Exemple.Domain.Exceptions
{
    [Serializable]
    internal class InvalidProductCodeException : Exception
    {
        public InvalidProductCodeException()
        {
        }

        public InvalidProductCodeException(string message) : base(message)
        {
        }

        public InvalidProductCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidProductCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
