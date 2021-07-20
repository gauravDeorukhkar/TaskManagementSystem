using EFDataAccess.Models;

using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Mapping
{
    public static class TaskToTaskResponseDtoMapper
    {
        public static TaskResponseDto Map(TaskDetails entity)
        {
            var task = new TaskResponseDto
            {
                TaskId = entity.TaskId,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                FinishDate = entity.FinishDate,
                State = entity.State,
                MainTaskId = (int)(entity.MainTaskId ?? 0)
            };
            return task;
            
        }
    }
}