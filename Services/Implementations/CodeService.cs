using Domain.Code;
using PasswordGenerator;
using System;
using System.Threading.Tasks;
using Infrastructure.Code;
using Infrastructure.Repositories.Generic;

namespace Services.Implementations
{
    public class CodeService : ICodeService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly CodeConfiguration _codeConfiguration;

        public CodeService(IGenericRepository genericRepository, CodeConfiguration codeConfiguration)
        {
            _genericRepository = genericRepository;
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
            var dbCode =  _genericRepository.GetOne<CodeModel>(p => p.Code == code && p.Phone == phone && p.ReasonType == codeReason);

            var result = dbCode?.IsActive == true && dbCode.DateExpiration > DateTime.UtcNow;

            if (dbCode != null && deactivate)
            {
                dbCode.IsActive = false;
                await _genericRepository.Update(dbCode);
            }

            return result;
        }

        public async Task<CodeModel> CreatePhoneCode(string phone, CodeReason codeReason)
        {
            var props = _codeConfiguration.Data[Enum.GetName(typeof(CodeReason), codeReason)];
            
            return await _genericRepository.Create(new CodeModel
            {
                Phone = phone,
                Code = GenerateCode(props.CodeLength),
                ReasonType = codeReason,
                DateExpiration = DateTime.UtcNow.AddMinutes(props.LifeTime)
            });
        }

        public async Task<bool> CheckEmailCodeExists(string email, string code, CodeReason codeReason, bool deactivate = true)
        {
            var dbCode =  _genericRepository.GetOne<CodeModel>(p => p.Code == code && p.Email == email && p.ReasonType == codeReason);

            if (deactivate)
            {
                dbCode.IsActive = false;
                await _genericRepository.Update(dbCode);
            }

            return dbCode?.IsActive == true && dbCode.DateExpiration > DateTime.UtcNow;
        }

        public async Task<CodeModel> CreateEmailCode(string email, CodeReason codeReason)
        {
            var props = _codeConfiguration.Data[Enum.GetName(typeof(CodeReason), codeReason)];

            return await _genericRepository.Create(new CodeModel
            {
                Email = email,
                Code = GenerateCode(props.CodeLength),
                ReasonType = codeReason,
                DateExpiration = DateTime.UtcNow.AddMinutes(props.LifeTime)
            });
        }
    }
}