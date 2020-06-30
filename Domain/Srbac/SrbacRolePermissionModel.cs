using Domain.Base;

namespace Domain.Srbac
{
    public class SrbacRolePermissionModel : BaseModel
    {
        public SrbacRoles Role { get; set;}
        public SrbacPermissions Permission { get; set;}
    }
}