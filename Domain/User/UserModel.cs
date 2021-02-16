using System;
using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Srbac;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.User
{
    public class UserModel : BaseModel
    {
        /// <summary>
        /// User first name
        /// </summary>
        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        [IndexColumn(IsClustered = false, IsUnique = false)]
        public string NameFirst { get; set; }

        /// <summary>
        /// User second name
        /// </summary>
        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        [IndexColumn(IsClustered = false, IsUnique = false)]
        public string NameSecond { get; set; }

        /// <summary>
        /// User patronymic name
        /// </summary>
        [MaxLength(20)]
        [IndexColumn(IsClustered = false, IsUnique = false)]
        public string NamePatronymic { get; set; }

        /// <summary>
        /// User phone
        /// </summary>
        [MaxLength(20)]
        [MinLength(10)]
        [Phone]
        [IndexColumn(IsClustered = false, IsUnique = true)]
        public string Phone { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        [Required]
        [EmailAddress]
        [IndexColumn(IsClustered = false, IsUnique = true)]
        public string Email { get; set; }

        /// <summary>
        /// User Password
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        [JsonIgnore]
        public string Password { get; set; }

        [IndexColumn(IsClustered = false, IsUnique = false)]
        public SrbacRoles Roles { get; set; }
    }
}