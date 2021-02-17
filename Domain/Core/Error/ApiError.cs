namespace Domain.Core.Error
{
    public class ApiError : IError
    {
        public string Error { get; }
        public string Field { get; }
        public ErrorCodes? ErrorCode { get; }

        public ApiError(ErrorCodes? errorCode, string field = null, string error = null)
        {
            Field = field;
            ErrorCode = errorCode;
        }
    }
}