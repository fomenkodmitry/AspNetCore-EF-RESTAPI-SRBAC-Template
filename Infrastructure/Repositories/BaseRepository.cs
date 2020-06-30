using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Filter;
using Domain.Srbac;
using Domain.User;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TModel> where TModel : BaseModel
    {
        protected readonly Context Context;
        private readonly AppSettingsConfiguration _appSettingsConfiguration;

        protected BaseRepository(Context context, AppSettingsConfiguration appSettingsConfiguration)
        {
            Context = context;
            _appSettingsConfiguration = appSettingsConfiguration;
        }

        public virtual async Task<TModel> GetById(Guid guid)
        {
            var data = Context.Set<TModel>().FirstOrDefaultAsync(u => u.Id == guid);
            return await data;
        }

        public async Task<TModel> Create(TModel data, Guid? creatorId = null)
        {
            data.CreatorId = creatorId;
            data.DateCreated = DateTime.UtcNow;
            data.IsActive ??= true;
            await Context.Set<TModel>().AddAsync(data);
            await Context.SaveChangesAsync();
            return data;
        }

        public async Task<TModel> Edit(TModel data)
        {
            await Context.SaveChangesAsync();
            return data;
        }

        public async Task<TModel> Delete(TModel model)
        {
            model.IsDelete = true;
            model.DateDelete = DateTime.Now;
            await Context.SaveChangesAsync();
            return model;
        }

        protected virtual IQueryable<TModel> ApplyPaging(IQueryable<TModel> source, FilterPagingDto paging)
        {
            paging ??= new FilterPagingDto {PageSize = _appSettingsConfiguration.DefaultPageSize};
            return source
                .Skip(paging.PageNumber * paging.PageSize)
                .Take(paging.PageSize);
        }
        
        /// <summary>
        /// Get filtered entities
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual IEnumerable<TModel> GetFiltered(BaseFilterDto filter)
        {
            var result =  Context.Set<TModel>()
                .AsQueryable();
            
            result = ApplyFilter(result, filter);
            
            var sort = filter.Sort;
            result = ApplySort(result, ref sort);
            result = ApplyPaging(result, filter.Paging);

            return result;
        }

        protected virtual IQueryable<TModel> ApplySort(IQueryable<TModel> source, ref FilterSortDto sort)
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

        protected virtual IQueryable<TModel> ApplyFilter(IQueryable<TModel> result, BaseFilterDto filter)
        {
            if (filter.IsActive.HasValue)
                result = result.Where(p => p.IsActive == filter.IsActive);

            return result;
        }


        public virtual async Task<int> GetCount(BaseFilterDto filter)
        {
            var result = Context.Set<TModel>().AsQueryable();

            result = ApplyFilter(result, filter);

            return await result.CountAsync();
        }
    }
}