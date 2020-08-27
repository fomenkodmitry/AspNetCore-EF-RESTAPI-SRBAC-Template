using System.Threading.Tasks;
using Domain.Filter;
using Domain.User;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.User
{
    public class UserRepository : BaseRepository<UserModel, BaseFilterDto>
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