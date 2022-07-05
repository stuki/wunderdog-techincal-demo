namespace Sula.Core.Exceptions
{
    public class SensorDoNotExistException : ExceptionWithCode
    {
        public override string Code => ErrorCode.SensorDoNotExist;
    }
}