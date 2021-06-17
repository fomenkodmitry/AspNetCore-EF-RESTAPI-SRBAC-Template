using System;
using System.Collections.Generic;
using Exceptions.ErrorCodes;

namespace Exceptions.Base
{
    public class AuthorizationException : ServiceExceptionBase
    {
        public AuthorizationException(string description, IDictionary<string, object> arguments = null)
            : base(CommonErrorCodes.InvalidAuthorization, description, arguments)
        {
        }

        public AuthorizationException(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : base(code, description, arguments)
        {
        }

        public AuthorizationException(
            string code,
            string description,
            Exception innerException,
            IDictionary<string, object> arguments = null)
            : base(code, description, innerException, arguments)
        {
        }
    }
}