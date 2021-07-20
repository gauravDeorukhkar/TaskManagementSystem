using System.Collections.Generic;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Models
{
    public class GetTasksQueryResponseDto
    {
        public IList<TaskResponseDto> Content { get; set; } = new List<TaskResponseDto>();
    }
}