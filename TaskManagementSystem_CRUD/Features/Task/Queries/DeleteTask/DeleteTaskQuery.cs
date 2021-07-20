using MediatR;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTask
{
    public class DeleteTaskQuery : IRequest<bool>
    {
        public int TaskId { get; set; }
    }
}