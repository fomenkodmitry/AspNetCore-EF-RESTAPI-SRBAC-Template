using System;
using System.Threading.Tasks;

namespace Domain.Token
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string userRole, Guid sessionId, string secretKey);
    }
}