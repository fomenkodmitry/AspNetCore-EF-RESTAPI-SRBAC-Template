using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.Srbac;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Domain.Audit
{
    public class AuditModel : BaseModel
    {
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public AuditOperationTypes OperationType { get; set; }
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public AuditStatuses Status { get; set; }
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public SrbacRoles Roles { get; set; }
        public string Comment { get; set; }
        [Column(TypeName = "jsonb")]
        public string ObjectDescription { get; set; }
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public Guid? ObjectId { get; set; }
    }
}