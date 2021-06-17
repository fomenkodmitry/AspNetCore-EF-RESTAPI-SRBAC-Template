using System;
using System.Collections.Generic;
using Exceptions.ErrorCodes;

namespace Exceptions.Base
{
    public class DataNotFoundException : ServiceExceptionBase
    {
        public DataNotFoundException(object id, string itemName)
            : base(CommonErrorCodes.DataNotFound, string.Format("{0} '{1}' не найден", (object) itemName, id), (IDictionary<string, object>) new Dictionary<string, object>()
            {
                {
                    nameof (id),
                    id
                }
            })
        {
        }

        public DataNotFoundException(string description, IDictionary<string, object> arguments = null)
            : base(CommonErrorCodes.DataNotFound, description, arguments)
        {
        }

        public DataNotFoundException(
            string code,
            string description,
            IDictionary<string, object> arguments = null)
            : base(code, description, arguments)
        {
        }

        public DataNotFoundException(
            string code,
            string description,
            Exception innerException,
            IDictionary<string, object> arguments = null)
            : base(code, description, innerException, arguments)
        {
        }
    }
}