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
using MimeKit;

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

        public async Task<string> CreateFromBase64(
            FilesTypes fileType,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            string contentBase64 = null,
            string fileName = null
        )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contentBase64))
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

                var filePath = Path.Combine(path, $"{fileModel.Id.ToString()}.{extension}");
                await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(contentBase64));
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
            var res = base64String.Substring(0, 5).ToUpper() switch
            {
                "IVBOR" => "png",
                "/9J/4" => "jpg",
                "/JVBER/4" => "pdf",
                _ => null
            };

            return res;
        }

        public async Task<string> GetFileUrl(Guid entityId, FilesTypes filesType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFileUrls(Guid entityId, FilesTypes filesType)
        {
            throw new NotImplementedException();
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
                    ContentType = MimeTypes.GetMimeType(path),
                    Content = await File.ReadAllBytesAsync(path)
                }
            );
        }
    }
}