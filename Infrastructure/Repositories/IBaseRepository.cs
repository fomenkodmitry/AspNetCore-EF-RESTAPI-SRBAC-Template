using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Filter;

namespace Infrastructure.Repositories
{
    public interface IBaseRepository<TModel, TFilter>
        where TModel : BaseModel
        where TFilter : BaseFilterDto
    {
        Task<TModel> GetById(Guid guid);
        Task Create(TModel data);
        Task Edit(TModel data);
        Task Delete(TModel data);
        Task<ICollection<TModel>> GetFiltered(TFilter filter);
        Task<int> GetCount(TFilter filter);
    }
}