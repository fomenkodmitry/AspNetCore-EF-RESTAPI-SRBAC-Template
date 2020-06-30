using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Authenticate;
using Domain.Base;
using Domain.Code;
using Domain.Error;
using Domain.Token;
using Domain.User;
using Infrastructure.Crypto;
using Infrastructure.Repositories.Code;
using Infrastructure.Repositories.Token;
using Infrastructure.Repositories.User;
using Microsoft.Extensions.Configuration;

namespace Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserRepository _userRepository;
        private readonly TokenRepository _tokenRepository;
        private readonly CryptoHelper _cryptoHelper;
        private readonly ITokenService _tokenService;
        private readonly string _secretKey;

        public AuthenticationService(
            IConfiguration configuration,
            UserRepository userRepository,
            CryptoHelper cryptoHelper,
            ITokenService tokenService,
            TokenRepository tokenRepository,
            CodeRepository codeRepository,
            ICodeService codeService
        )
        {
            _userRepository = userRepository;
            _cryptoHelper = cryptoHelper;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _secretKey = configuration["AppSettings:Secret"];
        }
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="requestDto" class="UserRegistrationResponseDto">UserRegistrationResponseDto</param>
        /// <returns></returns>
        public async Task<ResultContainer<UserRegistrationResponseDto>> Register(
            UserRegistrationRequestDto requestDto)
        {
            var userEmailExists = await _userRepository.GetByEmail(requestDto.Email);
            if (userEmailExists != null)
            {
                return new ResultContainer<UserRegistrationResponseDto>(ErrorCodes.UserEmailExists,
                    nameof(requestDto.Email));
            }

            var userPhoneExists = await _userRepository.GetByPhone(requestDto.Phone);
            if (userPhoneExists != null)
            {
                return new ResultContainer<UserRegistrationResponseDto>(ErrorCodes.UserEmailExists,
                    nameof(requestDto.Phone));
            }
            
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var res = await _userRepository.Create(new UserModel
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

            return new ResultContainer<UserRegistrationResponseDto>(
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
        public async Task<ResultContainer<UserLoginResponseDto>> Login(UserLoginRequestDto requestDto)
        {
            var user = await _userRepository.GetByEmail(requestDto.Email);
            if (user == null)
            {
                return new ResultContainer<UserLoginResponseDto>(ErrorCodes.IncorrectEmailOrPassword);
            }

            if (_cryptoHelper.GetHash(requestDto.Password) != user.Password)
            {
                return new ResultContainer<UserLoginResponseDto>(ErrorCodes.IncorrectEmailOrPassword);
            }

            var sessionId = Guid.NewGuid();
            var token = _tokenService.GenerateToken(user.Id, sessionId, _secretKey);

            await _tokenRepository.Create(new TokenModel
                {
                    Id = sessionId,
                    UserAgent = requestDto.UserAgent,
                    Token = token,
                    UserId = user.Id,
                    AppVersion = requestDto.AppVersion
                },
                user.Id.Value
            );
            return new ResultContainer<UserLoginResponseDto>(
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
        public async Task<ResultContainer<bool>> Logout(Guid sessionId)
        {
            var token = await _tokenRepository.GetById(sessionId);
            await _tokenRepository.Delete(token);
            return new ResultContainer<bool>(true);
        }
        /// <summary>
        /// Save push-token for firebase
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="pushToken"></param>
        /// <returns></returns>
        public async Task<ResultContainer<bool>> SavePushToken(Guid sessionId, string pushToken)
        {
            var tokenModel = await _tokenRepository.GetById(sessionId);
            if (tokenModel == null)
                return new ResultContainer<bool>(false);

            tokenModel.PushToken = pushToken;
            await _tokenRepository.Edit(tokenModel);

            return new ResultContainer<bool>(true);
        }
    }
}