﻿using Domain.Base;
using Domain.Code;
using Domain.Token;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 using System.ComponentModel.DataAnnotations.Schema;
 using System.Text.Json.Serialization;
 using Domain.Srbac;

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
        [JsonIgnore]
        public string Password { get; set; }
        
        public SrbacRoles Roles { get; set; }
        
        [JsonIgnore]
        public ICollection<TokenModel> Tokens { get; set; }
        [JsonIgnore]
        public ICollection<CodeModel> Codes { get; set; }
        [NotMapped]
        public IEnumerable<SrbacRoles> RolesList { get; set; }
    }
    
}