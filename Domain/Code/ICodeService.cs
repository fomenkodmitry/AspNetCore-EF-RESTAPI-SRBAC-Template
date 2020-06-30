using System;
using System.Threading.Tasks;

namespace Domain.Code
{
    public interface ICodeService
    {
        string GenerateCode(int len);
        string GeneratePassword(int len);
        /// <summary>
        /// Проверяет правильность кода и при необходимости деактивирует
        /// </summary>
        /// <param name="userId">Id пользователя, для которого выслан код</param>
        /// <param name="code">Код, отправленный пользователю</param>
        /// <param name="deactivate">True, если код нужно деактивировать после проверки</param>
        /// <returns>True, если код верный</returns>
        Task<bool> CheckPhoneCodeExists(string phone, string code, CodeReason codeReason, bool deactivate = true);

        Task<CodeModel> CreatePhoneCode(string phone, CodeReason codeReason);

        Task<bool> CheckEmailCodeExists(string email, string code, CodeReason codeReason, bool deactivate = true);

        Task<CodeModel> CreateEmailCode(string email, CodeReason codeReason);
    }
}