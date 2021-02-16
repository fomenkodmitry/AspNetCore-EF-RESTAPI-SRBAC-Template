using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;
using Domain.Base;
using Domain.Core;
using Domain.Error;
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
    /// <typeparam name="TKey"></typeparam>
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
            => BadRequest(ErrorCodes.WrongOperation);
        
        /// <summary>
        /// Return entity by id
        /// </summary>
        /// <param name="id">Id entity</param>
        /// <returns>Model entity</returns>
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TViewModel>> Get(Guid id)
            => BadRequest(ErrorCodes.WrongOperation);


        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="model">Description new entity</param>
        /// <returns>Model created entity</returns>
        [HttpPost]
        public virtual async Task<ActionResult<TViewModel>> Post([FromBody] TViewModel model)
            => BadRequest(ErrorCodes.WrongOperation);

        /// <summary>
        /// Edit entity
        /// </summary>
        /// <param name="model">Model with edit fields</param>
        /// <returns>Model edited entity</returns>
        [HttpPut]
        public virtual async Task<ActionResult<TViewModel>> Put([FromBody] TViewModel model)
            => BadRequest(ErrorCodes.WrongOperation);

        /// <summary>
        /// Delete entity by Id
        /// </summary>
        /// <param name="id">Id entity</param>
        /// <returns>Model deleted entity</returns>
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TViewModel>> Delete(Guid id)
            => BadRequest(ErrorCodes.WrongOperation);

        #region Response

        /// <summary>
        /// Response for bad request
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="property">Field(optional)</param>
        /// <returns>BadRequest, text error</returns>
        private static BadRequestGenericResult<TViewModel> BadRequest(ErrorCodes code, params string[] property)
            => new BadRequestGenericResult<TViewModel>(new ErrorContainer(code, string.Join(",", property)));

        /// <summary>
        /// Response
        /// </summary>
        /// <param name="result">Result</param>
        /// <returns>Container With result</returns>
        protected ActionResult<TViewModel> ProcessResult(ResultContainer<TViewModel> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }

        protected ActionResult<FilteredItemsCountDto> ProcessResult(ResultContainer<FilteredItemsCountDto> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }

        protected ActionResult<IEnumerable<TViewModel>> ProcessResult(ResultContainer<IEnumerable<TViewModel>> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }

        /// <summary>
        /// Response
        /// </summary>
        /// <param name="result">Result</param>
        /// <returns>Container With result</returns>
        protected ActionResult<TViewModel> ProcessResult<TM>(ResultContainer<TM> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }
        
        protected ActionResult<IEnumerable<TViewModel>> ProcessResult<TM>(ResultContainer<IEnumerable<TM>> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }
        
        #endregion
    }
}
