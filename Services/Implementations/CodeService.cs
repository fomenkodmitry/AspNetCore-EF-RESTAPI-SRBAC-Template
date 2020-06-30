using Domain.Code;
using Infrastructure.Repositories.Code;
using PasswordGenerator;
using System;
using System.Threading.Tasks;
using Infrastructure.Code;

namespace Services.Implementations
{
    public class CodeService : ICodeService
    {
        private CodeRepository _codeRepository;
        private CodeConfiguration _codeConfiguration;

        public CodeService(CodeRepository codeRepository, CodeConfiguration codeConfiguration)
        {
            _codeRepository = codeRepository;
            _codeConfiguration = codeConfiguration;
        }

        public string GenerateCode(int len)
        {
            return new Password().IncludeNumeric().LengthRequired(len).Next();
        }

        public string GeneratePassword(int len)
        {
            return new Password()
                .IncludeLowercase()
                .IncludeUppercase()
                .IncludeNumeric()
                .LengthRequired(len)
                .Next();
        }

        public async Task<bool> CheckPhoneCodeExists(string phone, string code, CodeReason codeReason, bool deactivate = true)
        {
            var dbCode = await _codeRepository.GetByPhone(phone, code, codeReason);

            var result = dbCode?.IsActive == true && dbCode.DateExpiration > DateTime.UtcNow;

            if (dbCode != null && deactivate)
            {
                dbCode.IsActive = false;
                await _codeRepository.Edit(dbCode);
            }

            return result;
        }

        public async Task<CodeModel> CreatePhoneCode(string phone, CodeReason codeReason)
        {
            var props = _codeConfiguration.Data[Enum.GetName(typeof(CodeReason), codeReason)];
            
            return await _codeRepository.Create(new CodeModel
            {
                Phone = phone,
                Code = GenerateCode(props.CodeLength),
                ReasonId = codeReason,
                DateExpiration = DateTime.UtcNow.AddMinutes(props.LifeTime)
            });
        }

        public async Task<bool> CheckEmailCodeExists(string email, string code, CodeReason codeReason, bool deactivate = true)
        {
            var dbCode = await _codeRepository.GetByEmail(email, code, codeReason);

            if (deactivate)
            {
                dbCode.IsActive = false;
                await _codeRepository.Edit(dbCode);
            }

            return dbCode?.IsActive == true && dbCode.DateExpiration > DateTime.UtcNow;
        }

        public async Task<CodeModel> CreateEmailCode(string email, CodeReason codeReason)
        {
            var props = _codeConfiguration.Data[Enum.GetName(typeof(CodeReason), codeReason)];

            return await _codeRepository.Create(new CodeModel
            {
                Email = email,
                Code = GenerateCode(props.CodeLength),
                ReasonId = codeReason,
                DateExpiration = DateTime.UtcNow.AddMinutes(props.LifeTime)
            });
        }
    }
}