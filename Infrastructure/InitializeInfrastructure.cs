using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Domain.FileStorage;
using Domain.Srbac;
using Infrastructure.FileStorage;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class InitializeInfrastructure
    {
        private readonly FileStorageConfiguration _fileStorageConfiguration;

        public InitializeInfrastructure(FileStorageConfiguration fileStorageConfiguration)
        {
            _fileStorageConfiguration = fileStorageConfiguration;
        }

        public InitializeInfrastructure FileStorage()
        {
            if (!Directory.Exists(_fileStorageConfiguration.AbsolutePath))
            {
                Directory.CreateDirectory(_fileStorageConfiguration.AbsolutePath);
            }

            foreach (var value in Enum.GetValues(typeof(FilesTypes)))
            {
                var path = Path.Combine(_fileStorageConfiguration.AbsolutePath, value.ToString());

                if (Directory.Exists(path))
                    continue;

                Directory.CreateDirectory(path);
            }

            return this;
        }
    }
}