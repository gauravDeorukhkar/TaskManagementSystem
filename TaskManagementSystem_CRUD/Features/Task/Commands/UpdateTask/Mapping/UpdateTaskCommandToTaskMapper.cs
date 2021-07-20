using EFDataAccess.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask
{
    public class UpdateTaskCommandToTaskMapper 
    { 
        public static TaskDetails Map(UpdateTaskCommand command)
        {
            var result = new TaskDetails
            {
                TaskId = command.TaskId,
                Name = command.Name,
                Description = command.Description,
                StartDate = command.StartDate,
                FinishDate = command.FinishDate,
                State = command.State,
                MainTaskId = command.MainTaskId == 0 ? null : command.MainTaskId
            };

            return result;
        }
    }
}