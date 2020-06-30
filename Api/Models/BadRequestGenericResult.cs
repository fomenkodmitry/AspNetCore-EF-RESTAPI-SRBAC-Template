﻿using Domain.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class BadRequestGenericResult<T> : BadRequestObjectResult where T : BaseModel
    {
        public BadRequestGenericResult([ActionResultObjectValue] object error) : base(error)
        {
        }
    }
}
