using Domain.Srbac;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Domain.Error;

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
        
        protected BadRequestObjectResult BadRequest(ErrorCodes code, string property)
            => BadRequest(new ErrorContainer(code, property));
    }
}