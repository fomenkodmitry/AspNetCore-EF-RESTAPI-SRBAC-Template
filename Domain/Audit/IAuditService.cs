using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Core.Result.Struct;
using Domain.Srbac;

namespace Domain.Audit
{
    public interface IAuditService
    {
        Task<Result<AuditModel>> Success(
            AuditOperationTypes operationType,
            string message,
            object auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        );

        Task<Result<AuditModel>> Error(
            AuditOperationTypes operationType,
            string message,
            AuditErrorObjectContainer auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        );

        Task<Result<AuditModel>> Cancel(
            AuditOperationTypes operationType,
            string message,
            object auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        );

        Result<IEnumerable<AuditModel>> GetAuditRecordByObjectId(Guid? id);
    }
}