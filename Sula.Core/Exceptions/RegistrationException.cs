using System;

namespace Sula.Core.Exceptions
{
    public sealed class RegistrationException : ExceptionWithCode
    {
        public RegistrationException(string code)
        {
            Code = code;
        }

        public RegistrationException(string code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}