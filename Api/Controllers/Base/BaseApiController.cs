using System;
using System.Threading.Tasks;
using Domain.Filter;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using IModel = Domain.Core.IModel;

namespace Api.Controllers.Base
{
    /// <summary>
    /// Base Api class
    /// </summary>
    /// <typeparam name="TFilter"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TCreateModel"></typeparam>
    /// <typeparam name="TUpdateModel"></typeparam>
    public abstract class BaseApiController<TViewModel, TCreateModel, TUpdateModel, TFilter> : BaseController 
        where TViewModel : class, IModel
        where TCreateModel : class, new()
        where TUpdateModel : class, new()
        where TFilter : BaseFilterDto
    {
        /// <summary>
        /// DI ctor
        /// </summary>
        /// <param name="userService"></param>
        protected BaseApiController(IUserService userService) : base (userService)
        {
        }

        /// <summary>
        /// Return all entities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<ActionResult<FilteredItemsDto<TViewModel>>> Get(TFilter filter)
            => Forbid();
        
        /// <summary>
        /// Return entity by id
        /// </summary>
        /// <param name="id">Id entity</param>
        /// <returns>Model entity</returns>
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TViewModel>> Get(Guid id)
            => Forbid();


        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="model">Description new entity</param>
        /// <returns>Model created entity</returns>
        [HttpPost]
        public virtual async Task<ActionResult<TViewModel>> Post([FromBody] TCreateModel model)
            => Forbid();

        /// <summary>
        /// Edit entity
        /// </summary>
        /// <param name="model">Model with edit fields</param>
        /// <returns>Model edited entity</returns>
        [HttpPut]
        public virtual async Task<ActionResult<TViewModel>> Put([FromBody] TUpdateModel model)
            => Forbid();

        /// <summary>
        /// Delete entity by Id
        /// </summary>
        /// <param name="id">Id entity</param>
        /// <returns>Model deleted entity</returns>
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TViewModel>> Delete(Guid id)
            => Forbid();
        
    }
}
