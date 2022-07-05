namespace Sula.Core.Exceptions
{
    public class CodeMismatchException : ExceptionWithCode
    {
        public override string Code => ErrorCode.CodeMismatch;
    }
}