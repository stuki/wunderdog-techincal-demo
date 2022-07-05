namespace Sula.Core.Exceptions
{
    public class LimitAlreadyExistException : ExceptionWithCode
    {
        public override string Code => ErrorCode.LimitAlreadyExists;
    }
}