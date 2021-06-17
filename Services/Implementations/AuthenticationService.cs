using System;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Authenticate;
using Domain.Mapper;
using Domain.Token;
using Infrastructure.Crypto;
using Infrastructure.Repositories.Token;
using Infrastructure.Repositories.User;
using Microsoft.Extensions.Configuration;

namespace Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly CryptoHelper _cryptoHelper;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenService _tokenService;
        private readonly string _secretKey;

        public AuthenticationService(
            IConfiguration configuration,
            CryptoHelper cryptoHelper,
            ITokenService tokenService,
            IUserRepository userRepository, 
            ITokenRepository tokenRepository)
        {
            _cryptoHelper = cryptoHelper;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _secretKey = configuration["AppSettings:Secret"];
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="requestDto" class="UserRegistrationResponseDto">UserRegistrationResponseDto</param>
        /// <returns></returns>
        public async Task<UserRegistrationResponseDto> Register(
            UserRegistrationRequestDto requestDto)
        {
            var userEmailExists = await _userRepository.GetByEmail(requestDto.Email);
            if (userEmailExists != null)
            {
                return Result<UserRegistrationResponseDto>.FromIError(new ApiError(ErrorCodes.UserEmailExists, nameof(requestDto.Email)));
            }

            var userPhoneExists = await _userRepository.GetByPhone(requestDto.Phone);
            if (userPhoneExists != null)
            {
                return Result<UserRegistrationResponseDto>.FromIError(new ApiError(ErrorCodes.UserEmailExists, nameof(requestDto.Phone)));
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = requestDto.MapToDto(_cryptoHelper.GetHash(requestDto.Password));
            await _userRepository.Create(user);
            
            var sessionId = Guid.NewGuid();
            var token = _tokenService.GenerateToken(user.Id, user.Roles, sessionId, _secretKey);

            await _tokenRepository.Create(new TokenModel
                {
                    Id = sessionId,
                    UserAgent = requestDto.UserAgent,
                    Token = token,
                    UserId = user.Id,
                    AppVersion = requestDto.AppVersion
                }
            );
            scope.Complete();

            return new UserRegistrationResponseDto
            {
                Id = user.Id,
                AuthToken = token
            };
        }

        /// <summary>
        /// User auth
        /// </summary>
        /// <param name="requestDto" class="UserLoginResponseDto">UserLoginResponseDto</param>
        /// <returns></returns>
        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto requestDto)
        {
            var user = await _userRepository.GetByEmail(requestDto.Email);
            if (user == null)
            {
                return Result<UserLoginResponseDto>.FromIError(new ApiError(ErrorCodes.IncorrectEmailOrPassword));
            }

            if (_cryptoHelper.GetHash(requestDto.Password) != user.Password)
            {
                return Result<UserLoginResponseDto>.FromIError(new ApiError(ErrorCodes.IncorrectEmailOrPassword));
            }

            var sessionId = Guid.NewGuid();
            var token = _tokenService.GenerateToken(user.Id, user.Roles, sessionId, _secretKey);

            await _tokenRepository.Create(new TokenModel
                {
                    Id = sessionId,
                    UserAgent = requestDto.UserAgent,
                    Token = token,
                    UserId = user.Id,
                    AppVersion = requestDto.AppVersion,
                    CreatorId = user.Id
                }
            );
            return new UserLoginResponseDto
            {
                Id = user.Id,
                AuthToken = token
            };
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <returns></returns>
        public async Task<bool> Logout(Guid sessionId)
        {
            var token = await _tokenRepository.GetById(sessionId);
            if (token == null)
                return true;
            await _tokenRepository.Delete(token);
            return true;
        }
    }
}