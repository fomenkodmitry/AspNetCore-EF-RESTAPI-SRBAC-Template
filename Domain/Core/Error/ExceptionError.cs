using System;

namespace Domain.Core.Error
{
    public class ExceptionError : IError
    {
        public string Error { get; }

        public ExceptionError(Exception exception) => Error = exception.Message;
    }
}