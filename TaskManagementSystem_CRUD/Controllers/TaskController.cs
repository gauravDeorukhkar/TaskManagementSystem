using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask.Models;
using TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask;
using TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask.Models;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTask;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTask.Models;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date;

namespace TaskManagementSystem_CRUD.Controllers
{

    // **Mediator Pattern for objects tobe  loosely coupled
    public class TaskController : ApiController
    {
        [HttpPost]
        [Route("CreateTask")]
        [ProducesResponseType(typeof(CreateTaskResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<CreateTaskResponse>> CreateTask(
            [FromBody] CreateTaskCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        [HttpPost]

        [Route("UpdateTask")]
        [ProducesResponseType(typeof(UpdateTaskResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<UpdateTaskResponse>> UpdateTask(
           [FromBody] UpdateTaskCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetTask")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskResponseDto>> GetClients(
            [FromQuery] int taskId)
        {
            var query = new GetTaskQuery
            {
                TaskId = taskId
            };
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetReport")]
        [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReport(
            [FromQuery] DateTime date)
        {
            var query = new GetTasksQuery
            {
                Date = date
            };
            var response = await Mediator.Send(query);

            var builder = new StringBuilder();
            builder.AppendLine("Name,Description,StartDate,FinishDate,State");
            foreach(var task in response.Content)
            {
                builder.AppendLine($"{task.Name},{task.Description},{task.StartDate},{task.FinishDate},{task.State}");
                
            }
            string fileName = "Tasks_" + date + ".csv";
            //return Ok(response);
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", fileName);        
        }

        [HttpDelete]
        [Route("DeleteTask")]
        public async void DeleteTask([FromQuery] int taskId)
        {
            
            var query = new DeleteTaskQuery
            {
                TaskId = taskId
            };
            var response = await Mediator.Send(query);
            
        }
    }
}