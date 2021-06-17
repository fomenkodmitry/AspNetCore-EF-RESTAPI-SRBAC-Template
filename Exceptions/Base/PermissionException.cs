using System;
using System.Collections.Generic;
using Exceptions.ErrorCodes;

namespace Exceptions.Base
{
    public class PermissionException : ServiceExceptionBase
    {
        public override LogErrorLevel LogLevel => LogErrorLevel.Error;

        public PermissionException(string description, IDictionary<string, object> arguments = null)
            : base(CommonErrorCodes.Forbidden, description, arguments)
        {
        }

        public PermissionException(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : base(code, description, arguments)
        {
        }

        public PermissionException(
            string code,
            string description,
            Exception innerException,
            IDictionary<string, object> arguments = null)
            : base(code, description, innerException, arguments)
        {
        }
    }
}