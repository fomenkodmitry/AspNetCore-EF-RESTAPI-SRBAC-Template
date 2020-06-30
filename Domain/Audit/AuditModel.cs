using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.Srbac;

namespace Domain.Audit
{
    public class AuditModel : BaseModel
    {
        public AuditOperationTypes OperationType { get; set; }
        public AuditStatuses Status { get; set; }
        public SrbacRoles Roles { get; set; }
        public string Comment { get; set; }
        [Column(TypeName = "jsonb")]
        public string ObjectDescription { get; set; }
        public Guid? ObjectId { get; set; }
    }
}