using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
    public class UserController : BaseApiController<UserModel>
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userService"></param>
        public UserController(IMapper mapper, IUserService userService) : base(userService, mapper)
        {
        }

        /// <summary>
        /// Get current user data
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult<UserModel> Self() => CurrentUser;

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user by id Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ActionResult<UserModel>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ActionResult<UserModel>> Post(UserModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="model">User model</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ActionResult<UserModel>> Put(UserModel model)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Detele user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<ActionResult<UserModel>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}