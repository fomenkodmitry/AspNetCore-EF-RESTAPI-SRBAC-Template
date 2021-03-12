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
            Directory.CreateDirectory(_fileStorageConfiguration.AbsolutePath);

            foreach (var value in Enum.GetValues(typeof(FilesTypes)))
            {
                var path = Path.Combine(_fileStorageConfiguration.AbsolutePath, value.ToString());
                Directory.CreateDirectory(path);
            }

            return this;
        }
    }
}