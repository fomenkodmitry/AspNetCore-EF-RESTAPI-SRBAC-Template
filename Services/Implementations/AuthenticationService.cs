using System;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Authenticate;
using Domain.Core.Error;
using Domain.Core.Result.Struct;
using Domain.Token;
using Domain.User;
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
        public async Task<Result<UserRegistrationResponseDto>> Register(
            UserRegistrationRequestDto requestDto)
        {
            var userEmailExists = await _userRepository.GetByEmail(requestDto.Email));
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
            await _userRepository.Create(new UserModel
                {
                    NameFirst = requestDto.NameFirst,
                    NameSecond = requestDto.NameSecond,
                    NamePatronymic = requestDto.NamePatronymic,
                    Password = _cryptoHelper.GetHash(requestDto.Password),
                    Phone = requestDto.Phone,
                    Email = requestDto.Email.ToLower()
                }
            );
            var sessionId = Guid.NewGuid();
            var token = _tokenService.GenerateToken(res.Id, sessionId, _secretKey);

            await _tokenRepository.Create(new TokenModel
                {
                    Id = sessionId,
                    UserAgent = requestDto.UserAgent,
                    Token = token,
                    UserId = res.Id,
                    AppVersion = requestDto.AppVersion
                }
            );
            scope.Complete();

            return new Result<UserRegistrationResponseDto>(
                new UserRegistrationResponseDto
                {
                    Id = res.Id,
                    AuthToken = token
                }
            );
        }

        /// <summary>
        /// User auth
        /// </summary>
        /// <param name="requestDto" class="UserLoginResponseDto">UserLoginResponseDto</param>
        /// <returns></returns>
        public async Task<Result<UserLoginResponseDto>> Login(UserLoginRequestDto requestDto)
        {
            var user = await _userRepository.GetByEmail(requestDto.Email));
            if (user == null)
            {
                return Result<UserLoginResponseDto>.FromIError(new ApiError(ErrorCodes.IncorrectEmailOrPassword));
            }

            if (_cryptoHelper.GetHash(requestDto.Password) != user.Password)
            {
                return Result<UserLoginResponseDto>.FromIError(new ApiError(ErrorCodes.IncorrectEmailOrPassword));
            }

            var sessionId = Guid.NewGuid();
            var token = _tokenService.GenerateToken(user.Id, sessionId, _secretKey);

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
            return new Result<UserLoginResponseDto>(
                new UserLoginResponseDto
                {
                    Id = user.Id,
                    AuthToken = token
                }
            );
        }

        /// <summary>
        /// Logout user
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <returns></returns>
        public async Task<Result<bool>> Logout(Guid sessionId)
        {
            var token = await _tokenRepository.GetById(sessionId);
            if (token == null)
                return new Result<bool>(true);
            await _tokenRepository.Delete(token);
            return new Result<bool>(true);
        }
    }
}