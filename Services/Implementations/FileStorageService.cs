using System;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Audit;
using Domain.Base;
using Domain.Error;
using Domain.FileStorage;
using Domain.Srbac;
using Infrastructure.FileStorage;
using Infrastructure.Repositories.File;

namespace Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly FileStorageConfiguration _fileStorageConfiguration;
        private readonly FileRepository _fileRepository;
        private readonly IAuditService _auditService;

        public FileStorageService(
            FileStorageConfiguration fileStorageConfiguration,
            FileRepository fileRepository,
            IAuditService auditService
        )
        {
            _fileStorageConfiguration = fileStorageConfiguration;
            _fileRepository = fileRepository;
            _auditService = auditService;
        }

        public async Task<ResultContainer<FileModel>> Create(
            FilesTypes fileType,
            string fileName,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            Stream file
        )
        {
            try
            {
                var name = Path.GetFileNameWithoutExtension(fileName);
                var extension = Path.GetExtension(fileName);

                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var fileModel = await _fileRepository.Create(new FileModel
                {
                    EntityType = fileType,
                    Extension = extension,
                    Name = name,
                    EntityId = entityId
                }, creatorId);

                var path = Path.Combine(_fileStorageConfiguration.AbsolutePath, fileType.ToString(),
                    entityId.ToString());
                Directory.CreateDirectory(path);

                var filePath = Path.Combine(path, fileModel.Id.ToString());
                await using var outputFileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(outputFileStream);
                await _auditService.Success(
                    AuditOperationTypes.CreateFile,
                    "",
                    fileModel,
                    fileModel.Id,
                    creatorId,
                    creatorRole
                );
                scope.Complete();
                return new ResultContainer<FileModel>(fileModel);
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

                return new ResultContainer<FileModel>(ErrorCodes.ServerError);
            }
        }


        public async Task<ResultContainer<FileModel>> Create(
            FilesTypes fileType,
            string fileName,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            string contentBase64
        )
        {
            try
            {
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

                var path = Path.Combine(_fileStorageConfiguration.AbsolutePath, fileType.ToString(),
                    entityId.ToString());
                Directory.CreateDirectory(path);
               
                var filePath = Path.Combine(path, fileModel.Id.ToString());
                await File.WriteAllTextAsync(filePath, contentBase64);

                await _auditService.Success(
                    AuditOperationTypes.CreateFile,
                    "",
                    fileModel,
                    fileModel.Id,
                    creatorId,
                    creatorRole
                );
                scope.Complete();
                return new ResultContainer<FileModel>(fileModel);
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

                return new ResultContainer<FileModel>(ErrorCodes.ServerError);
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
            string res = "";
            switch (data[0])
            {
                case "data:image/jpeg;base64":
                    res = "jpg";
                    break;
                case "data:image/png;base64":
                    res = "png";
                    break;
            }

            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }

            switch (base64String.Substring(0, 5).ToUpper())
            {
                case "IVBOR":
                    res = "png";
                    break;
                case "/9J/4":
                    res = "jpg";
                    break;
            }

            return res;
        }
    }
}