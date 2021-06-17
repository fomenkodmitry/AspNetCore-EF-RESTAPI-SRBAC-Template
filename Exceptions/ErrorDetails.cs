using System.Collections.Generic;

namespace Exceptions
{
    public class ErrorDetails
    {
        public ErrorDetails(string code, string description, IDictionary<string, object> arguments = null)
        {
            Code = code;
            Description = description;
            Arguments = arguments ?? new Dictionary<string, object>();
        }

        public string Code { get; }

        public string Description { get; }

        public IDictionary<string, object> Arguments { get; }

        public ErrorDetails WithArgument(string key, object value)
        {
            Arguments.Add(key, value);
            return this;
        }
    }
}