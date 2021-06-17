using System;

namespace Domain.Authenticate
{
    public class UserRegistrationResponseDto
    {
        public Guid? Id { get; set; }
        public string AuthToken { get; set; }
    }
}