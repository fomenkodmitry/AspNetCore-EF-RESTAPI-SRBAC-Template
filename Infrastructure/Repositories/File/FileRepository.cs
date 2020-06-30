﻿using Domain.FileStorage;
 using Infrastructure.AppSettings;
 using Infrastructure.Contexts;

namespace Infrastructure.Repositories.File
{
    public class FileRepository : BaseRepository<FileModel>
    {
        public FileRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }
    }
}