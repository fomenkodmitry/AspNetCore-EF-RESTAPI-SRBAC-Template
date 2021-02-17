using System;
using Domain.Core.Error;

namespace Domain.Core.Result.Struct
{
    public readonly struct Result<T>
    {
        public T Some { get; }
        public IError Error { get; }
        
        public bool IsSuccess => Error is null;

        public Result(T some)
        {
            if (some == null)
            {
                Some = default;
                //ToDo
                Error = new ExceptionError(new NullReferenceException("Null exception"));
            }

            Some = some;
            Error = default;
        }

        public Result(Exception error)
        {
            Error = new ExceptionError(error);
            Some = default;
        }

        public Result(IError error)
        {
            Error = error;
            Some = default;
        }

        public static Result<T> FromIError(IError error) => new(error);
    }
}