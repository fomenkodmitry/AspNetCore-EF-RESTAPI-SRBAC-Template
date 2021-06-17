using System.Threading.Tasks;
using Domain.Filter;
using Domain.User;

namespace Infrastructure.Repositories.User
{
    public interface IUserRepository: IBaseRepository<UserModel, BaseFilterDto>
    {
        Task<UserModel> GetByPhone(string phone);

        Task<UserModel> GetByEmail(string email);
    }
}