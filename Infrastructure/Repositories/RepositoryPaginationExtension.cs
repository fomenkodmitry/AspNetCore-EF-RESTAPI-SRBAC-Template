using System.Linq;
using Domain.Base;
using Domain.Filter;

namespace Infrastructure.Repositories
{
    public static class PaginationExtensionRepository
    {
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> source, FilterPagingDto paging) where TEntity : BaseModel
        {
            // get from config
            paging ??= new FilterPagingDto {PageSize = 10};
            return source
                .Skip(paging.PageNumber * paging.PageSize)
                .Take(paging.PageSize);
        }
    }
}