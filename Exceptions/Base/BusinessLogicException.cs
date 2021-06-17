using System;
using System.Collections.Generic;

namespace Exceptions.Base
{
    public class BusinessLogicException : ServiceExceptionBase
    {
        public BusinessLogicException(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : base(code, description, arguments)
        {
        }

        public BusinessLogicException(
            string code,
            string description,
            Exception innerException,
            IDictionary<string, object> arguments = null)
            : base(code, description, innerException, arguments)
        {
        }

        public BusinessLogicException(ErrorDetails errorDetails)
            : base(errorDetails)
        {
        }
    }
}