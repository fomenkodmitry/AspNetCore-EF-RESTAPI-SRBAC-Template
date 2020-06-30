using AutoMapper;
using Domain.Base;
using Domain.Error;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Filter;

namespace Api.Controllers
{
    /// <summary>
    /// Base Api class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseApiController<T> : BaseController where T : BaseModel
    {
        /// <summary>
        /// DI ctor
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        public BaseApiController(IUserService userService, IMapper mapper) : base (userService)
        {
            Mapper = mapper;
        }

        protected IMapper Mapper { get; }

        /// <summary>
        /// Return all entities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public abstract Task<ActionResult<IEnumerable<T>>> Get();

        /// <summary>
        /// Return entity by id
        /// </summary>
        /// <param name="id">Id entity</param>
        /// <returns>Model entity</returns>
        [HttpGet("{id}")]
        public abstract Task<ActionResult<T>> Get(Guid id);

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="model">Description new entity</param>
        /// <returns>Model created entity</returns>
        [HttpPost]
        public abstract Task<ActionResult<T>> Post([FromBody] T model);

        /// <summary>
        /// Edit entity
        /// </summary>
        /// <param name="model">Model with edit fields</param>
        /// <returns>Model edited entity</returns>
        [HttpPut]
        public abstract Task<ActionResult<T>> Put([FromBody] T model);

        /// <summary>
        /// Delete entity by Id
        /// </summary>
        /// <param name="id">Id entity</param>
        /// <returns>Model deleted entity</returns>
        [HttpDelete("{id}")]
        public abstract Task<ActionResult<T>> Delete(Guid id);

        #region Response

        /// <summary>
        /// Response for bad request
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="property">Field(optional)</param>
        /// <returns>BadRequest, text error</returns>
        protected BadRequestGenericResult<T> BadRequest(ErrorCodes code, params string[] property)
            => new BadRequestGenericResult<T>(new ErrorContainer(code, string.Join(",", property)));

        /// <summary>
        /// Response
        /// </summary>
        /// <param name="result">Result</param>
        /// <returns>Container With result</returns>
        protected ActionResult<T> ProcessResult(ResultContainer<T> result)
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

        protected ActionResult<IEnumerable<T>> ProcessResult(ResultContainer<IEnumerable<T>> result)
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
        protected ActionResult<T> ProcessResult<TM>(ResultContainer<TM> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }
        
        protected ActionResult<IEnumerable<T>> ProcessResult<TM>(ResultContainer<IEnumerable<TM>> result)
        {
            if (result.Error.HasValue)
                return BadRequest(result.Error.Value, result.ErrorField);

            return Ok(result.Result);
        }
        
        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // ignoring for put request (validate for edited fields)
            if (
                (context.ActionDescriptor as ControllerActionDescriptor).ActionName == nameof(Put) ||
                (context.ActionDescriptor as ControllerActionDescriptor).ActionName == nameof(Delete)
            )
                BypassValidation();
        }

        private void BypassValidation()
        {
            ModelState.Clear();
        }

        protected FilterPagingDto GetPaging()
        {
            return Request.Query.ContainsKey("pageNumber") ?
                new FilterPagingDto
                {
                    PageNumber = int.TryParse(Request.Query["pageNumber"].ToString(), out var pageNumber) ? pageNumber : 0,
                    PageSize = int.TryParse(Request.Query["pageSize"].ToString(), out var pageSize) ? pageSize : 0
                } : null;
        }

        protected FilterSortDto GetSort()
        {
            return Request.Query.ContainsKey("sortColumn") ?
                new FilterSortDto
                {
                    ColumnName = Request.Query["sortColumn"].ToString(),
                    IsDescending = bool.TryParse(Request.Query["descending"].ToString(), out var descending) && @descending
                } : null;
        }
    }
}
