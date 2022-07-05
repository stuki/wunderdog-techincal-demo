namespace Sula.Core.Exceptions
{
    public struct ErrorCode
    {
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string EmailNotMatching = "EMAILS_NOT_MATCHING";
        public const string ErrorCreatingUser = "ERROR_CREATING_USER";
        public const string SensorAlreadyExists = "SENSOR_ALREADY_EXISTS";
        public const string SensorDoNotExist = "SENSOR_DO_NOT_EXIST";
        public const string QueryIsEmpty = "QUERY_EMPTY";
        public const string CodeMismatch = "CODE_MISMATCH";
        public const string LimitDoNotExist = "LIMIT_DO_NOT_EXIST";
        public const string LimitAlreadyExists = "LIMIT_ALREADY_EXIST";
    }
}