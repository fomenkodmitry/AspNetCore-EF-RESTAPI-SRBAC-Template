﻿using Infrastructure.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    /// <summary>
    /// Settings and const application
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly AppSettingsConfiguration _appSettingsConfiguration;

        public SettingsController(AppSettingsConfiguration appSettingsConfiguration)
        {
            _appSettingsConfiguration = appSettingsConfiguration;
        }
    }
}
