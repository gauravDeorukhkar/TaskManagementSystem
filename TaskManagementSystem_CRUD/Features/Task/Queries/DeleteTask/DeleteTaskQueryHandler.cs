using EFDataAccess;
using EFDataAccess.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public DeleteTaskQueryHandler(ITaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> Handle(DeleteTaskQuery request, CancellationToken cancellationToken)
        {
            TaskDetails entity;
            if (request == null)
            {
                return false;
            }



            entity = _dbContext.Tasks.Where(t => t.TaskId == request.TaskId).FirstOrDefault();
            //var entity = await _dbContext.Tasks.FirstAsync(item => item.TaskId == request.TaskId);
            //entity = await _dbContext.Tasks.FindAsync(request.TaskId);
            await _dbContext.SaveChangesAsync(cancellationToken);

            //_dbContext.Entry(task).State = EntityState.Detached;

            EntityEntry<TaskDetails> persistedEntry;
            if (entity != null)
            {
               persistedEntry = _dbContext.Tasks.Remove(entity);

                await _dbContext.SaveChangesAsync(cancellationToken);
               return true;
            }

            return false;
            
        }
    }
}
