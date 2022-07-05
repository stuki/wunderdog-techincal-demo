using System;

namespace Sula.Core.Exceptions
{
    public class TokenRequestForbiddenException : Exception
    {
        public TokenRequestForbiddenException(string message) : base(message)
        {
        }
    }
}