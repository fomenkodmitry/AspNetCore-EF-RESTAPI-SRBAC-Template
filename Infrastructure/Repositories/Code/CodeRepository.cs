using System;
using System.Threading.Tasks;
using Domain.Code;
using Infrastructure.AppSettings;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Code
{
    public class CodeRepository : BaseRepository<CodeModel>
    {

        public async Task<CodeModel> GetByPhone(string phone, string code, CodeReason codeReason)
        {
            var result = Context.Codes.FirstOrDefaultAsync(c => c.Phone == phone && c.ReasonId == codeReason && c.Code == code);
            return await result;
        }
        public async Task<CodeModel> GetByEmail(string email, string code, CodeReason codeReason)
        {
            var result = Context.Codes.FirstOrDefaultAsync(c => c.Email == email && c.ReasonId == codeReason && c.Code == code);
            return await result;

        }
        public CodeRepository(Context context, AppSettingsConfiguration appSettingsConfiguration) : base(context, appSettingsConfiguration)
        {
        }
    }
}