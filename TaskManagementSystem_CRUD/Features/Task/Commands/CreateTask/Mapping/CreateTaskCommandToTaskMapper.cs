using EFDataAccess.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask
{
    public class CreateTaskCommandToTaskMapper
    { 
        public static TaskDetails Map(CreateTaskCommand command)
        {
            // Whenever a task is created it has goes in Planned state by default
            var result = new TaskDetails
            {

                Name = command.Name,
                Description = command.Description,
                StartDate = command.StartDate,
                FinishDate = command.FinishDate,
                State = "Planned",                  
                MainTaskId = command.MainTaskId == 0 ? null : command.MainTaskId

            };

            return result;
        }
    }
}