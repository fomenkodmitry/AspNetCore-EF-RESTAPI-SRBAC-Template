﻿using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
 using Domain.FileStorage;
 using Domain.Filter;
 using Infrastructure.AppSettings;
 using Infrastructure.Contexts;
 using Microsoft.EntityFrameworkCore;

 namespace Infrastructure.Repositories.File
{
    public class FileRepository : BaseRepository<FileModel, BaseFilterDto>
    {
        public FileRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }
        
        
        public async Task<FileModel> GetByFileType(FilesTypes filesTypes, Guid entityId)
            => await GetDataSet()
                .OrderByDescending(p => p.DateCreated)
                .FirstOrDefaultAsync(p => p.EntityId == entityId && p.EntityType == filesTypes);

        public IEnumerable<FileModel> GetByFileType(FilesTypes filesTypes, IEnumerable<Guid> entityIds)
            => GetDataSet()
                .Where(p => p.IsActive == true && p.EntityType == filesTypes && entityIds.Contains(p.EntityId));

        public async Task DeleteFilesByFileType(FilesTypes filesTypes, Guid entityId)
        {
            await GetDataSet()
                .Where(p => p.EntityId == entityId && p.EntityType == filesTypes)
                .ForEachAsync( a =>
                    {
                        a.IsActive = false;
                        a.IsDelete = true;
                    } 
                );
            await Context.SaveChangesAsync();
        }
    }
}