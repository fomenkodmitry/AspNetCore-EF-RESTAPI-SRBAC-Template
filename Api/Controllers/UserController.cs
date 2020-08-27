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
    public class UserController : BaseApiController<UserModel, BaseFilterDto>
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

    }
}