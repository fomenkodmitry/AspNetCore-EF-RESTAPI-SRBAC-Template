using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Audit;
using Domain.Base;
using Domain.Core.Result.Struct;
using Domain.Srbac;
using Infrastructure.Repositories;
using Newtonsoft.Json;

namespace Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly IGenericRepository _genericRepository;

        public AuditService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Result<AuditModel>> Success(
            AuditOperationTypes operationType,
            string message,
            object auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        )
        {
            var obj = JsonConvert.SerializeObject(
                auditObject, 
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
            var res = await _genericRepository.Create(
                new AuditModel
                {
                    OperationType = operationType,
                    Status = AuditStatuses.Success,
                    Roles = role,
                    Comment = message,
                    ObjectDescription = obj,
                    ObjectId = objectId,
                    CreatorId =  creatorId
                }
            );
            return new Result<AuditModel>(res);
        }

        public async Task<Result<AuditModel>> Error(
            AuditOperationTypes operationType,
            string message,
            AuditErrorObjectContainer auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        )
        {
            var obj = JsonConvert.SerializeObject(
                auditObject, 
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
            var res = await _genericRepository.Create(
                new AuditModel
                {
                    OperationType = operationType,
                    Status = AuditStatuses.Error,
                    Roles = role,
                    Comment = message,
                    ObjectDescription = obj,
                    ObjectId = objectId,
                    CreatorId = creatorId
                }
            );
            return new Result<AuditModel>(res);
        }

        public async Task<Result<AuditModel>> Cancel(
            AuditOperationTypes operationType,
            string message,
            object auditObject,
            Guid? objectId,
            Guid creatorId,
            SrbacRoles role
        )
        {
            var obj = JsonConvert.SerializeObject(
                auditObject, 
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
            var res = await _genericRepository.Create(
                new AuditModel
                {
                    OperationType = operationType,
                    Status = AuditStatuses.Cancelled,
                    Roles = role,
                    Comment = message,
                    ObjectDescription = obj,
                    ObjectId = objectId,
                    CreatorId = creatorId
                }
            );
            return new Result<AuditModel>(res);
        }

        public Result<IEnumerable<AuditModel>> GetAuditRecordByObjectId(Guid? id)
        {
            var res =  _genericRepository.Get<AuditModel>(p => p.ObjectId == id);
            return new Result<IEnumerable<AuditModel>>(res);
        }
    }
}