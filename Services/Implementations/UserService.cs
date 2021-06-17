using System;
using System.Threading.Tasks;
using Domain.User;
using Infrastructure.Repositories.User;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="guid" class="UserModel">UserModel</param>
        /// <returns></returns>
        public async Task<UserModel> GetById(Guid guid)
        {
            var res = await _userRepository.GetById(guid);
            return res;
        }
    }
}