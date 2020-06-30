using System.Collections.Generic;
using Domain.Error;

namespace Domain.Base
{
    public class BaseResponseDto
    {
        public ErrorCodes? Error { get; set; }
        public List<string> ErrorField { get; set; }
    }
}