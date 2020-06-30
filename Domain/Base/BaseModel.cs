using System;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace Domain.Base
{
    public abstract class BaseModel
    {
        public Guid? Id { get; set; }
        public bool? IsActive { get; set; } = true;
        [Index(IsClustered =  false, IsUnique = false)]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Index(IsClustered =  false, IsUnique = false)]
        public Guid? CreatorId { get; set; }
        public bool IsDelete { get; set; } = false;
        [Index(IsClustered =  false, IsUnique = false)]
        public DateTime? DateDelete { get; set; }
    }
}