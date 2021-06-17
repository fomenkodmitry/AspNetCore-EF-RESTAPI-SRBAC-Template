using Domain.Code;
using PasswordGenerator;

namespace Services.Implementations
{
    public class CodeService : ICodeService
    {
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