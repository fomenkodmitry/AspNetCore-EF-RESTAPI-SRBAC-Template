﻿﻿using System;
 using Domain.Base;

namespace Domain.Authenticate
{
    public class UserRegistrationResponseDto : BaseResponseDto
    {
        public Guid? Id { get; set; }
        public string AuthToken { get; set; }
    }
}