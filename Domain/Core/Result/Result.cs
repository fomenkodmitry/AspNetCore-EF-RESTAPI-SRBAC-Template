using System;
using Domain.Core.Error;

namespace Domain.Core.Result
{
    public static class Result
    {
        public readonly struct Some<T>
        {
            internal T Value { get; }

            internal Some(T value) => Value = value;
        }

        public readonly struct Failure
        {
            internal Exception Error { get; }

            public Failure(Exception error) => Error = error;
        }
        
        public readonly struct Failure<TError> where TError : IError
        {
            internal TError Error { get; }

            public Failure(TError error) => Error = error;
        }
    }
}