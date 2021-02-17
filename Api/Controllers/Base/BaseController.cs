using Domain.Srbac;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Core.Error;
using Domain.Core.Result.Struct;

namespace Api.Controllers.Base
{
    public class BaseController : Controller
    {
        
        public BaseController(IUserService userService)
        {
            UserService = userService;
        }

        #region Protected

        protected IUserService UserService { get; }
        
        private UserModel _currentUser;

        protected UserModel CurrentUser
        {
            get
            {
                if (_currentUser != null)
                    return _currentUser;

                _currentUser = UserService.GetById(Guid.Parse(User.FindFirstValue(ClaimTypes.Name))).Result;

                return _currentUser;
            }
        }

        protected SrbacRoles CurrentRole => Enum.Parse<SrbacRoles>(User.FindFirstValue(ClaimTypes.Role));

        #endregion
        
        /// <summary>
        /// Response
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Container With result</returns>
        protected async Task<ActionResult<T>> ProcessResultAsync<T>(Func<Task<Result<T>>> func)
        {
            try
            {
                var res = await func();
                if(res.IsSuccess)
                    return Ok(res.Some);

                return BadRequest(res.Error);
            }
            catch (Exception e)
            {
                return BadRequest(Result<T>.FromIError(new ExceptionError(e)));
            }
        }
        
        /// <summary>
        /// Response
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Container With result</returns>
        protected ActionResult<T> ProcessResult<T>(Func<Result<T>> func)
        {
            try
            {
                var res = func();
                if(res.IsSuccess)
                    return Ok(res.Some);

                return BadRequest(res.Error);
            }
            catch (Exception e)
            {
                return BadRequest(Result<T>.FromIError(new ExceptionError(e)));
            }
        }
        
    }
}