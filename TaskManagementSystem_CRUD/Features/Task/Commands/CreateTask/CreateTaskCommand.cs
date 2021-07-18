

using MediatR;
using System;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask
{

    public class CreateTaskCommand : IRequest<CreateTaskResponse>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string State { get; set; }

        public string MainTask { get; set; }


    }
}