using System.ComponentModel.DataAnnotations;
using Domain.User;

namespace Domain.Authenticate
{
    public class UserRegistrationRequestDto
    {
        /// <summary>
        /// User first name
        /// </summary>
        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        public string NameFirst { get; set; }
        /// <summary>
        /// User second name
        /// </summary>
        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        public string NameSecond { get; set; }
        /// <summary>
        /// User patronymic name
        /// </summary>
        [MaxLength(20)]
        public string NamePatronymic { get; set; }
        /// <summary>
        /// User phone
        /// </summary>
        [MaxLength(20)]
        [MinLength(10)]
        [Phone]
        public string Phone { get; set; }
        /// <summary>
        /// User Email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// User Password
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }
        /// <summary>
        /// User client
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// User app version
        /// </summary>
        public string AppVersion { get; set; }
    }
}