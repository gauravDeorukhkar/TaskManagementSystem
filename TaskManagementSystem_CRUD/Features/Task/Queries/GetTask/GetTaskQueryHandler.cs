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
    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskResponseDto>
    {
        private readonly ITaskDbContext _dbContext;
        public GetTaskQueryHandler(ITaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<TaskResponseDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var response = new TaskResponseDto();

            TaskDetails clientEntity;
            if(request.TaskId == 0)
            {
                return response;
            }
            
            clientEntity = await _dbContext.Tasks
                .Where(c => c.TaskId == request.TaskId)
                .FirstOrDefaultAsync(cancellationToken);

            if(clientEntity == null)
            {
                return response;
            }
            
            response = ExtTaskToExtTaskResponseDtoMapper.Map(clientEntity);
            
            return response;
            
        }
    }
}
