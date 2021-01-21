using Domain.Srbac;
using Infrastructure.Repositories;

namespace Services.Implementations
{
    public class SrbacService : ISrbacService
    {
        private readonly IGenericRepository _genericRepository;

        public SrbacService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public bool CheckPermission(SrbacRoles role, SrbacPermissions permission)
        {
            var res = _genericRepository.GetOne<SrbacRolePermissionModel>(
                p => p.Permission == permission && p.Role == role
            );
            return res != null;
        }
    }
}