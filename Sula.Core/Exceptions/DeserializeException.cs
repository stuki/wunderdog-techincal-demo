using System;
using System.Runtime.Serialization;

namespace Sula.Core.Exceptions
{
    [Serializable]
    public class DeserializeException : Exception
    {
        protected DeserializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DeserializeException()
        {
        }
    }
}