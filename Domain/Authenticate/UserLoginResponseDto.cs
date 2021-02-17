using System;
using Domain.Authenticate.Base;

namespace Domain.Authenticate
{
    public class UserLoginResponseDto : BaseResponseDto
    {
        public Guid? Id { get; set; }
        public string AuthToken { get; set; }
    }
}