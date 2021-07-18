using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskManagementSystem_CRUD.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));


        private IWebHostEnvironment _hostingEnvironment;

        private IWebHostEnvironment HostingEnvironment => _hostingEnvironment ??= (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));

        [HttpGet("env")]
        public IActionResult GetEnvironmentDetails()
        {
            var assembly = Assembly.GetExecutingAssembly().FullName;
            var result = new
            {
                Assembly = assembly,
                Environment = HostingEnvironment.EnvironmentName,
                Environment.MachineName,
                OS = $"{RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})"
            };
            return Ok(result);
        }
    }
}