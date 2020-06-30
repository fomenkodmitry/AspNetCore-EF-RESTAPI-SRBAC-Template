using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.User
{
    public interface IUserService
    {
        Task<UserModel> GetById(Guid guid);
    }
}