using MediatR;
using System;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date
{
    public class GetTasksQuery : IRequest<GetTasksQueryResponseDto>
    {
        public DateTime Date { get; set; }
    }
}