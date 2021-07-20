using EFDataAccess;
using EFDataAccess.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResponse>
    {
        private readonly ITaskDbContext _dbContext;
        private readonly ILogger<CreateTaskCommandHandler> _logger;

        public CreateTaskCommandHandler(
            ITaskDbContext dbContext,
            ILogger<CreateTaskCommandHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogDebug("The Create Task Request was empty.");
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
                
                _logger.LogDebug("Task Succefully Added with the name: [{Name}]", TaskEntity.Name);
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