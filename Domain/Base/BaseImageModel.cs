﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Base
{
    public class BaseImageModel : BaseModel
    {
        /// <summary>
        /// Изображение (На чтение - Url изображение, на запись - base64-encoded содержимое файла)
        /// </summary>
        [NotMapped] public string Image { get; set; }
    }
}