using Domain.Filter;
using Domain.Token;

namespace Infrastructure.Repositories.Token
{
    public interface ITokenRepository : IBaseRepository<TokenModel, BaseFilterDto>
    {
    }
}