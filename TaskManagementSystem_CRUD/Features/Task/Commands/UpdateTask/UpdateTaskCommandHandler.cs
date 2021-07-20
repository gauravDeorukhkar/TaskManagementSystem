using EFDataAccess;
using EFDataAccess.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskResponse>
    {
        private readonly ITaskDbContext _dbContext;
        private readonly ILogger<UpdateTaskCommandHandler> _logger;

        public UpdateTaskCommandHandler(ITaskDbContext dbContext,
            ILogger<UpdateTaskCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<UpdateTaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            
            if (request == null)
            {
                _logger.LogDebug("The Update Task Request was empty.");
                return new UpdateTaskResponse
                {
                    TaskId = -1,
                    StatusMessage = "Please provide a TaskId to be updated."
                };
            }
            

            var clientEntity = UpdateTaskCommandToTaskMapper.Map(request);
            EntityEntry<TaskDetails> persistedEntry;
            UpdateTaskResponse response;

            // ** Check if State has changed **
            _logger.LogDebug("Attemptimg to check if the State of Task has been Changed.");

            var task = await _dbContext.Tasks.FirstAsync(item => item.TaskId == clientEntity.TaskId);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            

            if (task.State != clientEntity.State)
            {

                // ** Check if the updated task is a Subtask and its status is changed to updated**
                // ** SubTasks Only **
                if (clientEntity.MainTaskId != null)
                {
                    _logger.LogDebug("Checking if Task State Needs tobe Updated with Subtask Update.");


                    if (clientEntity.State == "Completed")
                    {
                        var result = await _dbContext.Tasks
                                        .Where(c => c.MainTaskId == clientEntity.MainTaskId && c.TaskId != clientEntity.TaskId)
                                        .ToListAsync(cancellationToken);

                        if (result.Count != 0)
                        {
                            _logger.LogDebug("Checking the state of other subtask when current State is tobe updated as Completed");
                            //More Subtask present
                            foreach (var entity in result)
                            {

                                if (entity.State != "Complete")
                                {
                                    // **If any Subtask belonging to the same MainTask is not in Completed State, Update the Subtask**

                                    persistedEntry = _dbContext.Tasks.Update(clientEntity);
                                    await _dbContext.SaveChangesAsync(cancellationToken);

                                    _logger.LogDebug("Only Subtask by the name [{Name}] and [{TaskId}] is updated to Completed", clientEntity.Name,clientEntity.TaskId);

                                    response = new UpdateTaskResponse
                                    {
                                        TaskId = persistedEntry.Entity.TaskId,
                                        StatusMessage = "Task Updated Successfully."
                                    };


                                    return response;

                                }



                            }


                            persistedEntry = _dbContext.Tasks.Update(clientEntity);
                            await _dbContext.SaveChangesAsync(cancellationToken);

                            var task_1 = _dbContext.Tasks.FirstOrDefault(item => item.TaskId == clientEntity.MainTaskId);

                            // **Validate is the entry exist**
                            if (task_1 != null)
                            {
                                task_1.State = "Completed";

                                // **Update and Save changes in database**

                                _dbContext.Tasks.Update(task_1);
                                await _dbContext.SaveChangesAsync(cancellationToken);
                                _logger.LogDebug("All subtask are Completed so updating the state of Main Task to Completed by the id: [{TaskId}] ", task_1.TaskId);

                            }

                            response = new UpdateTaskResponse
                            {
                                TaskId = persistedEntry.Entity.TaskId,
                                StatusMessage = "Task Updated Successfully."
                            };

                            return response;


                        }

                        else
                        {
                            _logger.LogDebug("Updating the only Subtask with id [{TaskId}] and corresponding Maintask with id [{MainTaskId}] to Completed", clientEntity.TaskId,clientEntity.MainTaskId);

                            persistedEntry = _dbContext.Tasks.Update(clientEntity);
                            await _dbContext.SaveChangesAsync(cancellationToken);

                            var task_2 = _dbContext.Tasks.FirstOrDefault(item => item.TaskId == clientEntity.MainTaskId);

                            // **Validate is the entry exist**
                            if (task_2 != null)
                            {
                                task_2.State = "Completed";

                                // **Update and Save changes in database**

                                _dbContext.Tasks.Update(task_2);
                                await _dbContext.SaveChangesAsync(cancellationToken);
                            }

                            _logger.LogDebug("Successfully updated the Subtask with id [{TaskId}] and corresponding Maintask with id [{MainTaskId}] to Completed", clientEntity.TaskId, clientEntity.MainTaskId);


                            response = new UpdateTaskResponse
                            {
                                TaskId = persistedEntry.Entity.TaskId,
                                StatusMessage = "Task Updated Successfully."
                            };

                            return response;
                        }


                    }

                    if (clientEntity.State == "InProgress")
                    {
                        _logger.LogDebug("Updating the Subtask with id [{TaskId}] and corresponding Maintask with id [{MainTaskId}] to InProgress", clientEntity.TaskId, clientEntity.MainTaskId);

                        persistedEntry = _dbContext.Tasks.Update(clientEntity);
                        await _dbContext.SaveChangesAsync(cancellationToken);

                        var task_3 = _dbContext.Tasks.FirstOrDefault(item => item.TaskId == clientEntity.MainTaskId);

                        // **Validate is the entry exist**
                        if (task_3 != null)
                        {
                            task_3.State = "InProgress";

                            // **Update and Save changes in database**

                            _dbContext.Tasks.Update(task_3);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                        }

                        _logger.LogDebug("Successfully updated the Subtask with id [{TaskId}] and corresponding Maintask with id [{MainTaskId}] to Inprogress", clientEntity.TaskId, clientEntity.MainTaskId);


                        response = new UpdateTaskResponse
                        {
                            TaskId = persistedEntry.Entity.TaskId,
                            StatusMessage = "Task Updated Successfully."
                        };

                        return response;
                    }

                    if (clientEntity.State == "Planned")
                    {
                        var result = await _dbContext.Tasks
                                        .Where(c => c.MainTaskId == clientEntity.MainTaskId && c.TaskId != clientEntity.TaskId)
                                        .ToListAsync(cancellationToken);

                        if (result.Count != 0)
                        {
                            // **More Subtask present**
                            foreach (var entity in result)
                            {

                                if (entity.State == "InProgress")
                                {
                                    // **If any Subtask belonging to the same MainTask is in InProgress State, Update only the Subtask**

                                    persistedEntry = _dbContext.Tasks.Update(clientEntity);
                                    await _dbContext.SaveChangesAsync(cancellationToken);
                                    _logger.LogDebug("Successfully updated the Subtask with id [{TaskId}] to Planned", clientEntity.TaskId);

                                    response = new UpdateTaskResponse
                                    {
                                        TaskId = persistedEntry.Entity.TaskId,
                                        StatusMessage = "Task Updated Successfully."
                                    };


                                    return response;

                                }



                            }


                            persistedEntry = _dbContext.Tasks.Update(clientEntity);
                            await _dbContext.SaveChangesAsync(cancellationToken);

                            var task_4 = _dbContext.Tasks.FirstOrDefault(item => item.TaskId == clientEntity.MainTaskId);

                            // **Validate is the entry exist**
                            if (task_4 != null)
                            {
                                task_4.State = "Planned";

                                // **Update and Save changes in database**

                                _dbContext.Tasks.Update(task_4);
                                await _dbContext.SaveChangesAsync(cancellationToken);
                            }
                            _logger.LogDebug("Successfully updated the Subtask with id [{TaskId}] and corresponding Maintask with id [{MainTaskId}] to Planned", clientEntity.TaskId, clientEntity.MainTaskId);

                            response = new UpdateTaskResponse
                            {
                                TaskId = persistedEntry.Entity.TaskId,
                                StatusMessage = "Task Updated Successfully."
                            };

                            return response;


                        }

                        else
                        {
                            persistedEntry = _dbContext.Tasks.Update(clientEntity);
                            await _dbContext.SaveChangesAsync(cancellationToken);

                            var task_5 = _dbContext.Tasks.FirstOrDefault(item => item.TaskId == clientEntity.MainTaskId);

                            // **Validate is the entry exist**
                            if (task_5 != null)
                            {
                                task_5.State = "Planned";

                                // **Update and Save changes in database**

                                _dbContext.Tasks.Update(task_5);
                                await _dbContext.SaveChangesAsync(cancellationToken);
                            }
                            response = new UpdateTaskResponse
                            {
                                TaskId = persistedEntry.Entity.TaskId,
                                StatusMessage = "Task Updated Successfully."
                            };

                            return response;
                        }
                    }
                    
                    persistedEntry = _dbContext.Tasks.Update(clientEntity);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    response = new UpdateTaskResponse
                    {
                        TaskId = persistedEntry.Entity.TaskId,
                        StatusMessage = "Task Updated Successfully."
                    };


                    return response;
                }


                // ** Check if there is no Subtask under the Maintask been Updated **
                // ** MainTask Only **
                else 
                {
                    var result = await _dbContext.Tasks
                        .Where(c => c.MainTaskId == clientEntity.TaskId)
                        .ToListAsync(cancellationToken);

                    if (result.Count != 0)
                    {
                        _logger.LogDebug("Update the Subtasks belonging to this MainTask with id: [{TaskId}]", clientEntity.TaskId);

                        response = new UpdateTaskResponse
                        {
                            TaskId = -1,
                            StatusMessage = "Please Update the Subtasks belonging to this MainTask"
                        };

                        return response;
                    }


                    persistedEntry = _dbContext.Tasks.Update(clientEntity);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    
                    response = new UpdateTaskResponse
                    {
                        TaskId = persistedEntry.Entity.TaskId,
                        StatusMessage = "Task Updated Successfully."
                    };
                    return response;
                }


            }
            // **If state has not changed **
            else
            {
                _logger.LogDebug("No State change so update the given task with id : [{TaskId}]", clientEntity.TaskId);

                persistedEntry = _dbContext.Tasks.Update(clientEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogDebug("Succefully updated the given task with id : [{TaskId}]", clientEntity.TaskId);
                response = new UpdateTaskResponse
                {
                    TaskId = persistedEntry.Entity.TaskId,
                    StatusMessage = "Task Updated Successfully."
                };
                return response;
            }


            
        }
    }

}