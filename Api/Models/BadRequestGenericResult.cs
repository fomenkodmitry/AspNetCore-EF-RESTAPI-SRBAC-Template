using Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Models
{
    public class BadRequestGenericResult<TViewModel> : BadRequestObjectResult where TViewModel : class, IModel
    {
        public BadRequestGenericResult([ActionResultObjectValue] object error) : base(error)
        {
        }
    }
}
