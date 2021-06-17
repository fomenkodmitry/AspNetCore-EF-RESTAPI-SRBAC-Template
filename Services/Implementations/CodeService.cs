using Domain.Code;
using PasswordGenerator;
using System;
using System.Threading.Tasks;
using Infrastructure.Code;

namespace Services.Implementations
{
    public class CodeService : ICodeService
    {
        private readonly CodeConfiguration _codeConfiguration;

        public CodeService(CodeConfiguration codeConfiguration)
        {
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
    }
}