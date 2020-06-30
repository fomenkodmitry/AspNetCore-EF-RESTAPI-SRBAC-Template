using Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Code
{
    public class CodeModel : BaseModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }

        [Required] 
        public DateTime DateExpiration { get; set; }

        [Required] 
        public string Code { get; set; }
        [Required] 
        public CodeReason ReasonId { get; set; }
    }
}