using System;
using System.Collections.Generic;
using Exceptions.ErrorCodes;

namespace Exceptions.Base
{
    public class InternalServiceException : ServiceExceptionBase
    {
        public override LogErrorLevel LogLevel => LogErrorLevel.Error;

        public InternalServiceException(string description, IDictionary<string, object> arguments = null)
            : base(CommonErrorCodes.InternalServiceError, description, arguments)
        {
        }

        public InternalServiceException(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : base(code, description, arguments)
        {
        }

        public InternalServiceException(
            string code,
            string description,
            Exception innerException,
            IDictionary<string, object> arguments = null)
            : base(code, description, innerException, arguments)
        {
        }
    }
}