using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repos.Lib
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger<ControllerBase> _logger;

        public BaseController(ILogger<ControllerBase> logger)
        {
            _logger = logger;
        }
    }
}
