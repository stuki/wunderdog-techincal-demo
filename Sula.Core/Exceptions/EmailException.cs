using System;

namespace Sula.Core.Exceptions
{
    public class EmailException : Exception
    {
        public EmailException(string message) : base(message)
        {
        }
    }
}