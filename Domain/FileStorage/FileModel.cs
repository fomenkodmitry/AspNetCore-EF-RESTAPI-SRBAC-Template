﻿using System;
using Domain.Base;

namespace Domain.FileStorage
{
    public class FileModel : BaseModel
    {
        /// <summary>
        /// Имя файла (User-friendly)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Расширение
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// Тип файлов
        /// </summary>
        public FilesTypes EntityType { get; set; }
        /// <summary>
        /// ID Сущности
        /// </summary>
        public Guid EntityId { get; set; }
    }
}