using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Srbac;

namespace Domain.Audit
{
    public interface IAuditService
    {
        Task<ResultContainer<AuditModel>> Success(
            AuditOperationTypes operationType,
            string message,
            object auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        );

        Task<ResultContainer<AuditModel>> Error(
            AuditOperationTypes operationType,
            string message,
            AuditErrorObjectContainer auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        );

        Task<ResultContainer<AuditModel>> Cancel(
            AuditOperationTypes operationType,
            string message,
            object auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        );

        ResultContainer<IEnumerable<AuditModel>> GetAuditRecordByObjectId(Guid? id);
    }
}