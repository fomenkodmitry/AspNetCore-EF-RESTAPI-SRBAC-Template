using System;
using System.Collections.Generic;
using System.Linq;

namespace Exceptions.Base
{
    public abstract class ServiceExceptionBase : Exception, ILoggerLevel
    {
        public virtual LogErrorLevel LogLevel => LogErrorLevel.Warning;

        public ErrorDetails ErrorDetails { get; }

        protected ServiceExceptionBase(
            string code,
            string description,
            Exception ex,
            IDictionary<string, object> arguments = null)
            : base(description, ex)
        {
            if (arguments != null && arguments.Any())
            {
                foreach (var keyValuePair in arguments)
                    Data.Add(keyValuePair.Key, keyValuePair.Value);
            }
            ErrorDetails = new ErrorDetails(code, description, arguments);
        }

        protected ServiceExceptionBase(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : this(code, description, null, arguments)
        {
        }

        protected ServiceExceptionBase(ErrorDetails errorDetails)
            : base(errorDetails?.Description)
        {
            ErrorDetails = errorDetails ?? throw new ArgumentNullException(nameof (errorDetails));
            if (ErrorDetails.Arguments == null)
                return;
            foreach (KeyValuePair<string, object> keyValuePair in ErrorDetails.Arguments)
                Data.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}