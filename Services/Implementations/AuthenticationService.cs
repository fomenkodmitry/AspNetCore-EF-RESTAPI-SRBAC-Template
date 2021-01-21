using System;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Authenticate;
using Domain.Base;
using Domain.Error;
using Domain.Token;
using Domain.User;
using Infrastructure.Crypto;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericRepository _genericRepository;
        private readonly CryptoHelper _cryptoHelper;
        private readonly ITokenService _tokenService;
        private readonly string _secretKey;

        public AuthenticationService(
            IConfiguration configuration,
            CryptoHelper cryptoHelper,
            ITokenService tokenService,
            IGenericRepository genericRepository)
        {
            _cryptoHelper = cryptoHelper;
            _tokenService = tokenService;
            _genericRepository = genericRepository;
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
            var userEmailExists = _genericRepository.GetOne<UserModel>(p => p.Email == requestDto.Email);
            if (userEmailExists != null)
            {
                return new ResultContainer<UserRegistrationResponseDto>(ErrorCodes.UserEmailExists,
                    nameof(requestDto.Email));
            }

            var userPhoneExists = _genericRepository.GetOne<UserModel>(p => p.Email == requestDto.Phone);
            if (userPhoneExists != null)
            {
                return new ResultContainer<UserRegistrationResponseDto>(ErrorCodes.UserEmailExists,
                    nameof(requestDto.Phone));
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var res = await _genericRepository.Create(new UserModel
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

            await _genericRepository.Create(new TokenModel
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
            var user = _genericRepository.GetOne<UserModel>(p => p.Email == requestDto.Email);
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

            await _genericRepository.Create(new TokenModel
                {
                    Id = sessionId,
                    UserAgent = requestDto.UserAgent,
                    Token = token,
                    UserId = user.Id,
                    AppVersion = requestDto.AppVersion,
                    CreatorId = user.Id.Value
                }
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
            var token = await _genericRepository.GetById<TokenModel>(sessionId);
            if (token == null)
                return new ResultContainer<bool>(true);
            await _genericRepository.Remove(token);
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
            var tokenModel = await _genericRepository.GetById<TokenModel>(sessionId);
            if (tokenModel == null)
                return new ResultContainer<bool>(false);

            tokenModel.PushToken = pushToken;
            await _genericRepository.Update(tokenModel);

            return new ResultContainer<bool>(true);
        }
    }
}