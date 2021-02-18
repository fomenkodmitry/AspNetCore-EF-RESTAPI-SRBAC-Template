namespace Domain.Core.Error
{
    /// <summary>
    /// Error api container
    /// </summary>
    public class ApiError : IError
    {
        /// <summary>
        /// Error text
        /// </summary>
        public string Error { get; }
        /// <summary>
        /// Field with error
        /// </summary>
        public string Field { get; }
        /// <summary>
        /// Error code (const for client)
        /// </summary>
        public ErrorCodes? ErrorCode { get; }
        public int? ErrorCodeId => (int?) ErrorCode;

        public ApiError(ErrorCodes? errorCode, string field = null, string error = null)
        {
            Field = field;
            ErrorCode = errorCode;
            Error = error;
        }
    }
}