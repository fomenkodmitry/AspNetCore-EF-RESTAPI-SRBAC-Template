﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Srbac;

namespace Domain.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> Create(
            FilesTypes fileType,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            string contentBase64 = null,
            string fileName = null,
            bool deactivateOldFiles = true
        );

        Task<string> GetFileUrl(Guid entityId, FilesTypes filesType);
        Task<ResultContainer<FileStorageDto>> GetFileById(Guid fileId);
        Dictionary<Guid, string> GetFileUrls(IEnumerable<Guid> entityIds, FilesTypes filesType);
        IEnumerable<string> GetFileUrls(Guid entityId, FilesTypes filesType);
        IEnumerable<TModel> GetImagesWithModel<TModel>(IEnumerable<TModel> result, FilesTypes filesType)
            where TModel : BaseImageModel;
    }
}