using System;

namespace Domain.Authenticate
{
    public class UserLoginResponseDto
    {
        public Guid? Id { get; set; }
        public string AuthToken { get; set; }
    }
}