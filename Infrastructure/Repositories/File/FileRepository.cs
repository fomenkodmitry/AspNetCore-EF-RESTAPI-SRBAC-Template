using Domain.FileStorage;
using Domain.Filter;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories.File
{
    public class FileRepository : BaseRepository<FileModel, BaseFilterDto>
    {
        public FileRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context,
            appSettingsConfiguration)
        {
        }
    }
}