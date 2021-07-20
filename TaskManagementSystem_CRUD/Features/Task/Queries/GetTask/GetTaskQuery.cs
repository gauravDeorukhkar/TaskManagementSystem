using MediatR;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTask
{
    public class GetTaskQuery : IRequest<TaskResponseDto>
    {
        public int TaskId { get; set; }
    }
}