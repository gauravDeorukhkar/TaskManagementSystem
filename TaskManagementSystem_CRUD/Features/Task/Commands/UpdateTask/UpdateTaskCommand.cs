using System;
using MediatR;
using TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest<UpdateTaskResponse>
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string State { get; set; }
        public int MainTaskId { get; set; }
    }
}