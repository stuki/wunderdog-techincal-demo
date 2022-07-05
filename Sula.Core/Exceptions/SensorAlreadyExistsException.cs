namespace Sula.Core.Exceptions
{
    public class SensorAlreadyExistsException : ExceptionWithCode
    {
        public override string Code => ErrorCode.SensorAlreadyExists;
    }
}