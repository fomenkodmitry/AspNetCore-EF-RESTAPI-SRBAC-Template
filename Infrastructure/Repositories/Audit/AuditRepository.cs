using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Audit;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Audit
{
    public class AuditRepository : BaseRepository<AuditModel>
    {
        public AuditRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }

        public IEnumerable<AuditModel> GetAuditRecordByObjectId(Guid? guid)
        {
            return Context.Audits.Where(p => p.ObjectId == guid);
        }
    }
}