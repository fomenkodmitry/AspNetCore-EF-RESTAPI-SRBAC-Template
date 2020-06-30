using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.User;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.User
{
    public class UserRepository : BaseRepository<UserModel>
    {
        public UserRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }

        public async Task<UserModel> GetByPhone(string phone)
        {
            var user = Context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
            return await user;
        }

        public async Task<UserModel> GetByEmail(string email)
        {
            var user = Context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return await user;
        }
    }
}