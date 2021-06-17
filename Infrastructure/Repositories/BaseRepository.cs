using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Filter;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TModel, TFilter>
        where TModel : BaseModel
        where TFilter : BaseFilterDto
    {
        protected readonly Context Context;
        private readonly AppSettingsConfiguration _appSettingsConfiguration;

        protected BaseRepository(Context context, AppSettingsConfiguration appSettingsConfiguration)
        {
            Context = context;
            _appSettingsConfiguration = appSettingsConfiguration;
        }

        public virtual async Task<TModel> GetById(Guid guid)
            => await GetDataSet().FirstOrDefaultAsync(u => u.Id == guid && u.IsDelete == false);

        public async Task Create(TModel data)
        {
            data.DateCreated = DateTime.UtcNow;
            data.IsActive ??= true;
            data.DateUpdated = DateTime.UtcNow;
            await Context.Set<TModel>().AddAsync(data);
            await Context.SaveChangesAsync();
        }

        public async Task Edit(TModel data)
        {
            data.DateUpdated = DateTime.UtcNow;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(TModel data)
        {
            data.IsDelete = true;
            data.DateDelete = DateTime.Now;
            data.DateUpdated = DateTime.UtcNow;
            Context.Update(data);
            await Context.SaveChangesAsync();
        }

        protected virtual IQueryable<TModel> ApplyPaging(IQueryable<TModel> source, FilterPagingDto paging)
        {
            paging ??= new FilterPagingDto {PageSize = _appSettingsConfiguration.DefaultPageSize};
            return source
                .Skip(paging.PageNumber * paging.PageSize)
                .Take(paging.PageSize);
        }

        protected virtual IQueryable<TModel> GetDataSet()
            => Context.Set<TModel>().AsNoTracking();

        /// <summary>
        /// Get filtered entities
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual async Task<ICollection<TModel>> GetFiltered(TFilter filter)
        {
            var result = GetDataSet();

            result = ApplyFilter(result, filter);

            result = ApplySort(result, filter.Sort);
            result = ApplyPaging(result, filter.Paging);

            return await result.ToListAsync();
        }

        protected virtual IQueryable<TModel> ApplySort(IQueryable<TModel> source, FilterSortDto sort)
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

        protected virtual IQueryable<TModel> ApplyFilter(IQueryable<TModel> result, TFilter filter)
        {
            if (filter.IsActive.HasValue)
                result = result.Where(p => p.IsActive == filter.IsActive);

            result = result.Where(p => p.IsDelete == false);

            return result;
        }


        public virtual async Task<int> GetCount(TFilter filter)
        {
            var result = Context.Set<TModel>().AsNoTracking();
            result = ApplyFilter(result, filter);

            return await result.CountAsync();
        }
    }
}