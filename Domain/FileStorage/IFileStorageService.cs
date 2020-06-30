﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Srbac;

namespace Domain.FileStorage
{
    public interface IFileStorageService
    {
        public Task<ResultContainer<FileModel>> Create(
            FilesTypes fileType, 
            string fileName,
            Guid entityId, 
            Guid creatorId,
            SrbacRoles creatorRole,
            Stream file
        );

        public Task<ResultContainer<FileModel>> Create(
            FilesTypes fileType,
            string fileName,
            Guid entityId,
            Guid creatorId,
            SrbacRoles creatorRole,
            string contentBase64
        );
    }
}