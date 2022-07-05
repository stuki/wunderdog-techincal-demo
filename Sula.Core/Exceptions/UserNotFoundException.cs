namespace Sula.Core.Exceptions
{
    public class UserNotFoundException : ExceptionWithCode
    {
        public override string Code => ErrorCode.UserNotFound;
    }
}