using System.Threading.Tasks;
using Domain.Filter;
using Domain.Token;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.Token
{
    public class TokenRepository : BaseRepository<TokenModel, BaseFilterDto>, ITokenRepository
    {
        public TokenRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }
 
    }
}