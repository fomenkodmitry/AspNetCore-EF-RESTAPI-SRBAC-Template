using System;
using Domain.Core.Error;

namespace Domain.Core.Result.Struct
{
    /// <summary>
    /// Generic Result Container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct Result<T>
    {
        /// <summary>
        /// Result
        /// </summary>
        public T Some { get; }
        /// <summary>
        /// Error
        /// </summary>
        public IError Error { get; }
        /// <summary>
        /// Operation is success
        /// </summary>
        public bool IsSuccess => Error is null;

        public Result(T some)
        {
            if (some == null)
            {
                Some = default;
                //ToDo
                Error = new ExceptionError(new NullReferenceException($"Null {nameof(some)} exception"));
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