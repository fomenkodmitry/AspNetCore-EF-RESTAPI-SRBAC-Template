using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Audit;
using Domain.Base;
using Domain.Error;
using Domain.FileStorage;
using Domain.Srbac;
using Infrastructure.FileStorage;
using Infrastructure.Host;
using Infrastructure.Repositories.File;

namespace Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly FileStorageConfiguration _fileStorageConfiguration;
        private readonly FileRepository _fileRepository;
        private readonly IAuditService _auditService;
        private readonly HostConfiguration _hostConfiguration;

        public FileStorageService(
            FileStorageConfiguration fileStorageConfiguration,
            FileRepository fileRepository,
            IAuditService auditService,
            HostConfiguration hostConfiguration
        )
        {
            _fileStorageConfiguration = fileStorageConfiguration;
            _fileRepository = fileRepository;
            _auditService = auditService;
            _hostConfiguration = hostConfiguration;
        }

        public async Task<string> Create(
            FilesTypes fileType,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            string contentBase64 = null,
            string fileName = null,
            bool deactivateOldFiles = true
        )
        {
            try
            {
                if(string.IsNullOrWhiteSpace(contentBase64))
                    return null;
                
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffffffK");
                
                var name = Path.GetFileNameWithoutExtension(fileName);
                var extension = GetFileExtension(contentBase64);

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var fileModel = await _fileRepository.Create(new FileModel
                {
                    EntityType = fileType,
                    Extension = extension,
                    Name = name,
                    EntityId = entityId
                }, creatorId);

                var path = Path.Combine(
                    _fileStorageConfiguration.AbsolutePath,
                    fileType.ToString(),
                    entityId.ToString()
                );
                Directory.CreateDirectory(path);

                var filePath = Path.Combine(path, fileModel.Id.ToString());
                await File.WriteAllTextAsync(filePath, contentBase64);
                await _fileRepository.DeleteFilesByFileType(fileType, entityId);
                await _auditService.Success(
                    AuditOperationTypes.CreateFile,
                    "",
                    fileModel,
                    fileModel.Id,
                    creatorId,
                    creatorRole
                );
                scope.Complete();
                return await GetFileUrl(entityId, fileType);
            }
            catch (Exception e)
            {
                await _auditService.Error(
                    AuditOperationTypes.CreateFile,
                    e.Message,
                    new AuditErrorObjectContainer
                    {
                        Model = new FileModel
                        {
                            EntityType = fileType,
                            Name = fileName,
                            EntityId = entityId
                        },
                        Error = e
                    },
                    null,
                    creatorId,
                    creatorRole
                );
                throw;
            }
        }

        /// <summary>
        /// To demonstrate extraction of file extension from base64 string.
        /// </summary>
        /// <param name="base64String">base64 string.</param>
        /// <returns>Henceforth file extension from string.</returns>
        private static string GetFileExtension(string base64String)
        {
            var data = base64String.Split(",");
            var res = data[0] switch
            {
                "data:image/jpeg;base64" => "jpg",
                "data:image/png;base64" => "png",
                _ => ""
            };

            if (!string.IsNullOrEmpty(res))
                return res;

            res = base64String.Substring(0, 5).ToUpper() switch
            {
                "IVBOR" => "png",
                "/9J/4" => "jpg",
                _ => res
            };

            return res;
        }

        public async Task<string> GetFileUrl(Guid entityId, FilesTypes filesType)
        {
            var res = await _fileRepository.GetByFileType(filesType, entityId);
            return res == null ? null : $"{_hostConfiguration.HostNameWithProtocol}/api/File/{res.Id}";
        }

        public Dictionary<Guid, string> GetFileUrls(IEnumerable<Guid> entityIds, FilesTypes filesType)
        {
            var res = _fileRepository.GetByFileType(filesType, entityIds);

            return res?.AsEnumerable() //  дальнейшие операции осуществляются с коллекцией, а не на SQL
                .GroupBy(p => p.EntityId) //  на случай двух файлов по одному entity
                .ToDictionary(
                    p => p.Key, //  ключ - EntityId 
                    p => //  значение - FileId
                    {
                        //  Последний файл - первый сверху по дате desc
                        var lastFile = p.OrderByDescending(f => f.DateCreated).FirstOrDefault().Id.Value;
                        return $"{_hostConfiguration.HostNameWithProtocol}/api/File/{lastFile}";
                    });
        }

        public IEnumerable<string> GetFileUrls(Guid entityId, FilesTypes filesType)
        {
            var res = _fileRepository.GetByFileType(filesType, new[] {entityId});
            return res?.ToList().Select( p => $"{_hostConfiguration.HostNameWithProtocol}/api/File/{p.Id}");
        }

        public async Task<ResultContainer<FileStorageDto>> GetFileById(Guid fileId)
        {
            var res = await _fileRepository.GetById(fileId);

            if (res == null)
                return new ResultContainer<FileStorageDto>(null);

            var path = Path.Combine(
                _fileStorageConfiguration.AbsolutePath,
                res.EntityType.ToString(),
                res.EntityId.ToString(),
                res.Id.ToString()
            );

            if (!File.Exists(path))
                return new ResultContainer<FileStorageDto>(null);

            return new ResultContainer<FileStorageDto>(
                new FileStorageDto()
                {
                    ContentType = @$"image/{res.Extension}",
                    Content = Convert.FromBase64String(await File.ReadAllTextAsync(path))
                }
            );
        }
      
        public IEnumerable<TModel> GetImagesWithModel<TModel>(IEnumerable<TModel> result, FilesTypes filesType) where TModel : BaseImageModel
        {
            var images = GetFileUrls(result.Select(p => p.Id.Value), filesType);
            result = result.Select(element =>
            {
                if (images.TryGetValue(element.Id.Value, out var image))
                    element.Image = image;
                return element;
            });
            return result;
        }
    }
}