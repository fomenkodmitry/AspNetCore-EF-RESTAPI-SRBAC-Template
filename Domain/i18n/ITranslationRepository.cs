using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.i18n
{
    public interface ITranslationRepository
    {
        Task<TranslationModel> GetTranslation(TranslationObjectTypes objectType, int objectId, Languages language );
        Task<IEnumerable<TranslationModel>> GetTranslations(TranslationObjectTypes objectType, Languages language);
        Task<IEnumerable<TranslationModel>> GetTranslations(TranslationObjectTypes objectType);
    }
}
