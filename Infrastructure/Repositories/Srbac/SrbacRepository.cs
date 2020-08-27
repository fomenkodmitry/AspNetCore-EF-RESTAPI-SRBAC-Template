using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Filter;
using Domain.Srbac;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Srbac
{
    public class SrbacRepository : BaseRepository<SrbacRolePermissionModel, BaseFilterDto>
    {
        public IEnumerable<SrbacRolePermissionModel> RolesPermissions { get; set; }

        public SrbacRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
            RolesPermissions = Context.SrbacRolePermissions.ToList();
        }
    }
}