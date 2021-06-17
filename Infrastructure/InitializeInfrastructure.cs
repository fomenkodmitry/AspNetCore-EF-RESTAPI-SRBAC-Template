using System;
using System.IO;
using Domain.FileStorage;
using Infrastructure.FileStorage;

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