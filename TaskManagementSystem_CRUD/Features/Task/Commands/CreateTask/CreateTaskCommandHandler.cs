using EFDataAccess;
using EFDataAccess.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResponse>
    {
        private readonly ITaskDbContext _dbContext;

        public CreateTaskCommandHandler(ITaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new CreateTaskResponse
                {
                    TaskId = -1,
                    StatusMessage = "Null request passed to CreateTaskCommand"
                };
            }

            var TaskEntity = CreateTaskCommandToTaskMapper.Map(request);

            EntityEntry<TaskDetails> persistedEntry;

            // try catch block just for demonstration purpose. Its not used in this app as TaskId are unique each time a new task is created.
            try
            {
                persistedEntry = await _dbContext.Tasks.AddAsync(TaskEntity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                throw new Exception("This task is already registered on TMS.");
            }
            var response = new CreateTaskResponse
            {
                TaskId = persistedEntry.Entity.TaskId,
                StatusMessage = "Task Created Successfully."
            };

            return response;
        }
    }
}