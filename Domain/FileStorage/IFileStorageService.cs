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
        Task<string> CreateFromBase64(
            FilesTypes fileType,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            string contentBase64 = null,
            string fileName = null
        );

        Task<string> GetFileUrl(Guid entityId, FilesTypes filesType);
        Task<ResultContainer<FileStorageDto>> GetFileById(Guid fileId);
        IEnumerable<string> GetFileUrls(Guid entityId, FilesTypes filesType);

    }
}