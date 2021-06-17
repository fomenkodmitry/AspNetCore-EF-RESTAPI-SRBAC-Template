using System;
using System.Threading.Tasks;

namespace Domain.Authenticate
{
    public interface IAuthenticationService
    {
        Task<UserRegistrationResponseDto> Register(UserRegistrationRequestDto requestDto);
        Task<UserLoginResponseDto> Login(UserLoginRequestDto requestDto);
        Task<bool> Logout(Guid sessionId);

    }
}