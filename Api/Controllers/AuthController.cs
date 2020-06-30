using System;
using System.Security.Claims;
using Domain.Authenticate;
using Domain.Error;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;

namespace Api.Controllers
{
    /// <summary>
    /// Login user
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// DI ctor
        /// </summary>
        /// <param name="authenticateService"></param>
        /// <param name="userService"></param>
        public AuthController(IAuthenticationService authenticateService, IUserService userService) : base(userService)
        {
            _authenticationService = authenticateService;
        }

        /// <summary>
        /// User login
        /// </summary>
        /// <param name="requestDto" class="UserLoginRequestDto"></param>
        /// <param name="userAgent">Header - User-Agent</param>
        /// <param name="appVersion">Header - X-Application-Version</param>
        /// <returns class="UserLoginResponseDto">UserLoginResponseDto</returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<UserLoginResponseDto>> Login(
            [FromBody] UserLoginRequestDto requestDto,
            [FromHeader(Name = "User-Agent")] string userAgent,
            [FromHeader(Name = "X-Application-Version")]
            string appVersion
        )
        {
            requestDto.UserAgent = userAgent;
            requestDto.AppVersion = appVersion;
            var result = await _authenticationService.Login(requestDto);

            if (result.Error != null)
            {
                var error = result.Error ?? ErrorCodes.NotFound;
                return BadRequest(error, result.ErrorField);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="requestDto">Reg user data</param>
        /// <param name="userAgent">Header - User-Agent</param>
        /// <param name="appVersion">Header - X-Application-Version</param>
        /// <returns class = "UserRegistrationResponseDto">UserRegistrationResponseDto</returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<UserRegistrationResponseDto>> Registration(
            [FromBody] UserRegistrationRequestDto requestDto,
            [FromHeader(Name = "User-Agent")] string userAgent,
            [FromHeader(Name = "X-Application-Version")]
            string appVersion
        )
        {
            requestDto.UserAgent = userAgent;
            requestDto.AppVersion = appVersion;
            var result = await _authenticationService.Register(requestDto);

            if (result.Error != null)
                return BadRequest(result.Error.Value, result.ErrorField);
            
            return Ok(result.Result);
        }
        
        
        /// <summary>
        /// Save push token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>200 OK</returns>
        [HttpPut("[action]")]
        public async Task<ActionResult> SavePushToken(SavePushTokenDto model)
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.Sid), out var sessionId))
                await _authenticationService.SavePushToken(sessionId, model.PushToken);

            return Ok();
        }
        
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>200 OK</returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> Logout()
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.Sid), out var sessionId))
                await _authenticationService.Logout(sessionId);
            return Ok();
        }
        
    }
}