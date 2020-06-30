﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Authenticate
{
    public class UserPasswordForgotRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}