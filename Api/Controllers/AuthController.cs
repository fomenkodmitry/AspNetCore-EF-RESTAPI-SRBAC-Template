﻿using System;
using System.Security.Claims;
using Domain.Authenticate;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Api.Controllers.Base;

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
            return await ProcessResultAsync(() =>_authenticationService.Login(requestDto));
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
            return await ProcessResultAsync(() => _authenticationService.Register(requestDto));
        }


        /// <summary>
        /// Save firebase push token
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