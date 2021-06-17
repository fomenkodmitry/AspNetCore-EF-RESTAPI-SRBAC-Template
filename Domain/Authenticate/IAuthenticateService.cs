using System;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Core.Result.Struct;

namespace Domain.Authenticate
{
    public interface IAuthenticationService
    {
        Task<Result<UserRegistrationResponseDto>> Register(UserRegistrationRequestDto requestDto);
        Task<Result<UserLoginResponseDto>> Login(UserLoginRequestDto requestDto);
        Task<Result<bool>> Logout(Guid sessionId);

    }
}