using System;
using System.Threading.Tasks;
using Domain.User;
using Infrastructure.Repositories;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository _genericRepository;

        public UserService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="guid" class="UserModel">UserModel</param>
        /// <returns></returns>
        public async Task<UserModel> GetById(Guid guid)
        {
            var res = await _genericRepository.GetById<UserModel>(guid);
            return res;
        }
    }
}