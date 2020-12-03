﻿﻿using System;
using Domain.Base;
  using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

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
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public FilesTypes EntityType { get; set; }
        /// <summary>
        /// ID Сущности
        /// </summary>
        [IndexColumn(IsClustered =  false, IsUnique = false)]
        public Guid EntityId { get; set; }
    }
}