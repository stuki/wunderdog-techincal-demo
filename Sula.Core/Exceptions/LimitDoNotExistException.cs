namespace Sula.Core.Exceptions
{
    public class LimitDoNotExistException : ExceptionWithCode
    {
        public override string Code => ErrorCode.LimitDoNotExist;
    }
}