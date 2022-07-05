namespace Sula.Core.Exceptions
{
    public class QueryEmptyException : ExceptionWithCode
    {
        public override string Code => ErrorCode.QueryIsEmpty;
    }
}