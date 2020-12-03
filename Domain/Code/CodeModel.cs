using Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.Code
{
    public class CodeModel : BaseModel
    {
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public string Phone { get; set; }
     
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public string Email { get; set; }

        [IndexColumn(IsClustered =  false, IsUnique = false)]
        [Required] 
        public DateTime DateExpiration { get; set; }

        [IndexColumn(IsClustered =  false, IsUnique = false)]
        [Required] 
        public string Code { get; set; }
        
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        [Required] 
        public CodeReason ReasonType { get; set; }
    }
}