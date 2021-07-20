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
using TaskManagementSystem_CRUD.Features.Queries.GetTask.Mapping;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTask.Models;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTask
{
    public class DeleteTaskQueryHandler : IRequestHandler<DeleteTaskQuery, bool>
    {
        private readonly ITaskDbContext _dbContext;
        private readonly ILogger<DeleteTaskQueryHandler> _logger;

        public DeleteTaskQueryHandler(ITaskDbContext dbContext,
            ILogger<DeleteTaskQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteTaskQuery request, CancellationToken cancellationToken)
        {
            TaskDetails entity;

            // **Needs tobe more polished and looked into**
            if (request == null)
            {
                return false;
            }


            _logger.LogDebug("Attempting to Fetch task with id: [{TaskId}]", request.TaskId);
            
            
            entity = _dbContext.Tasks.Where(t => t.TaskId == request.TaskId).FirstOrDefault();
            await _dbContext.SaveChangesAsync(cancellationToken);
            //var entity = await _dbContext.Tasks.FirstAsync(item => item.TaskId == request.TaskId);
            //entity = await _dbContext.Tasks.FindAsync(request.TaskId);


            //_dbContext.Entry(task).State = EntityState.Detached;

            EntityEntry<TaskDetails> persistedEntry;
            if (entity != null)
            {
                _logger.LogDebug("Attempting to Delete task with id: [{TaskId}]", entity.TaskId);
                persistedEntry = _dbContext.Tasks.Remove(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogDebug("Succefully Deleted task with id: [{TaskId}]", entity.TaskId);
                return true;
            }

            return false;
            
        }
    }
}
