using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Srbac;
using Domain.User;

namespace Domain.Token
{
    public class TokenModel: BaseModel
    {
        public string AppVersion { get; set; }
        public string UserAgent { get; set; }

        [Required]
        public string Token { get; set; }

        public string PushToken { get; set; }

        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public SrbacRoles Role { get; set; }

        public UserModel User { get; set; }
    }
}