using EFDataAccess;
using EFDataAccess.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public UpdateTaskCommandHandler(ITaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<UpdateTaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            
            if (request == null)
            {
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

            var task = await _dbContext.Tasks.FirstAsync(item => item.TaskId == clientEntity.TaskId);
            //_dbContext.Entry(task).State = EntityState.Detached;
            await _dbContext.SaveChangesAsync(cancellationToken);
            //

            if (task.State != clientEntity.State)
            {

                // ** Check if the updated task is a Subtask and its status is changed to updated**
                // ** SubTasks Only **
                if (clientEntity.MainTaskId != null)
                {
                    if (clientEntity.State == "Completed")
                    {
                        var result = await _dbContext.Tasks
                                        .Where(c => c.MainTaskId == clientEntity.MainTaskId && c.TaskId != clientEntity.TaskId)
                                        .ToListAsync(cancellationToken);

                        if (result.Count != 0)
                        {
                            //More Subtask present
                            foreach (var entity in result)
                            {

                                if (entity.State != "Complete")
                                {
                                    // **If any Subtask belonging to the same MainTask is not in Completed State, Update the Subtask**

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
                            //More Subtask present
                            foreach (var entity in result)
                            {

                                if (entity.State == "InProgress")
                                {
                                    // **If any Subtask belonging to the same MainTask is not in Completed State, Update the Subtask**

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
            // **If State has not changed **
            else
            {
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
    }

}