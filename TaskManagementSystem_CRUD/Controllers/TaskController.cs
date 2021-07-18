using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask.Models;

namespace TaskManagementSystem_CRUD.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ApiController
    {
        [HttpPost]
        
        public async Task<ActionResult<CreateTaskResponse>> CreateClient(
            [FromBody] CreateTaskCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}