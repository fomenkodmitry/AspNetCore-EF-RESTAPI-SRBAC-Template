using System;
using System.Collections.Generic;
using Exceptions.ErrorCodes;

namespace Exceptions.Base
{
    public class InvalidArgumentsException : ServiceExceptionBase
    {
        public InvalidArgumentsException(string description, IDictionary<string, object> arguments = null)
            : base(CommonErrorCodes.InvalidRequest, description, arguments)
        {
        }

        public InvalidArgumentsException(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : base(code, description, arguments)
        {
        }

        public InvalidArgumentsException(
            string code,
            string description,
            Exception innerException,
            IDictionary<string, object> arguments = null)
            : base(code, description, innerException, arguments)
        {
        }
    }
}