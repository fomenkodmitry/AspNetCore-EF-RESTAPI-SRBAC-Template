using System;
using Toolbelt.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.Base
{
    public abstract class BaseModel
    {
        public Guid? Id { get; set; }
        public bool? IsActive { get; set; }
   
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public DateTime DateCreated { get; set; }
    
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public Guid? CreatorId { get; set; }
     
        public bool IsDelete { get; set; }
  
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public DateTime? DateDelete { get; set; }
        
        
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public DateTime DateUpdated { get; set; }
    }
}