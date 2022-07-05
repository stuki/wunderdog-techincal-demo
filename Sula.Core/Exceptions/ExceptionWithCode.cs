using System;

namespace Sula.Core.Exceptions
{
    public class ExceptionWithCode : Exception
    {
        public virtual string Code { get; set; }
        
        protected ExceptionWithCode(string message, Exception innerException) : base(message, innerException)
        {
        }
        
        protected ExceptionWithCode(string message) : base(message)
        {
        }

        protected ExceptionWithCode()
        {
        }
    }
}