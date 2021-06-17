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
            return this;
        }
    }
}