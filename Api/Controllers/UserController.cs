using System;
using Api.Controllers.Base;
using AutoMapper;
using Domain.Filter;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Users
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseApiController<UserModel, UserCreateDto, UserUpdateDto, BaseFilterDto>
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService) : base(userService)
        {
        }

        /// <summary>
        /// Get current user data
        /// </summary>
        /// <returns>Current user</returns>
        [HttpGet("[action]")]
        public ActionResult<UserModel> Self() => CurrentUser;
    }
}