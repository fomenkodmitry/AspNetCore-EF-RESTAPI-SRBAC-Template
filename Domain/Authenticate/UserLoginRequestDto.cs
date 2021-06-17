using System.ComponentModel.DataAnnotations;

namespace Domain.Authenticate
{
    public class UserLoginRequestDto
    {
        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }
        /// <summary>
        /// Http User Agent
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// Application Version
        /// </summary>
        public string AppVersion { get; set; }
    }
}