using System.Threading.Tasks;
using Domain.Code;
using Domain.Filter;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Code
{
    public class CodeRepository : BaseRepository<CodeModel, BaseFilterDto>
    {

        public async Task<CodeModel> GetByPhone(string phone, string code, CodeReason codeReason)
        {
            var result = GetDataSet().FirstOrDefaultAsync(c => c.Phone == phone && c.ReasonType == codeReason && c.Code == code);
            return await result;
        }
        public async Task<CodeModel> GetByEmail(string email, string code, CodeReason codeReason)
        {
            var result = GetDataSet().FirstOrDefaultAsync(c => c.Email == email && c.ReasonType == codeReason && c.Code == code);
            return await result;

        }
        public CodeRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }
    }
}