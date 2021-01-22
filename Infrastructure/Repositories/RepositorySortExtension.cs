using System.Linq;
using Domain.Base;
using Domain.Filter;

namespace Infrastructure.Repositories
{
    public static class RepositorySortExtension
    {
        public static IQueryable<TEntity> ApplySort<TEntity>(this IQueryable<TEntity> source, ref FilterSortDto sort) where TEntity : BaseModel
        {
            sort ??= new FilterSortDto
            {
                ColumnName = nameof(BaseModel.DateCreated),
                IsDescending = true
            };

            if (sort.ColumnName == nameof(BaseModel.DateCreated))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.DateCreated)
                    : source.OrderBy(p => p.DateCreated);

            if (sort.ColumnName == nameof(BaseModel.IsActive))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.IsActive)
                    : source.OrderBy(p => p.IsActive);

            if (sort.ColumnName == nameof(BaseModel.DateCreated))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.DateCreated)
                    : source.OrderBy(p => p.DateCreated);

            if (sort.ColumnName == nameof(BaseModel.IsActive))
                return sort.IsDescending
                    ? source.OrderByDescending(p => p.IsActive)
                    : source.OrderBy(p => p.IsActive);

            return source;
        }
    }
}