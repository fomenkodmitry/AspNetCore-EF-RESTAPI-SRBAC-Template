using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Filter;
using Domain.Srbac;
using Domain.Token;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Token
{
    public class TokenRepository : BaseRepository<TokenModel, BaseFilterDto>
    {
        public TokenRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context,
            appSettingsConfiguration)
        {
        }

        public Task<bool> HasActiveToken(Guid tokenId, Guid userId, SrbacRoles role)
        {
            return GetDataSet()
                .Where(p => p.UserId == userId && p.Role == role && p.Id == tokenId && p.IsActive == true)
                .AnyAsync();
        }
    }
}