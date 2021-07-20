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
    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskResponseDto>
    {
        private readonly ITaskDbContext _dbContext;
        private readonly ILogger<GetTaskQueryHandler> _logger;

        public GetTaskQueryHandler(ITaskDbContext dbContext,
            ILogger<GetTaskQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TaskResponseDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var response = new TaskResponseDto();

            TaskDetails clientEntity;
            if(request.TaskId == 0)
            {
                return response;
            }
            _logger.LogDebug("Attempting to get a task with id: [{TaskId}]", request.TaskId);
            
            clientEntity = await _dbContext.Tasks
                .Where(c => c.TaskId == request.TaskId)
                .FirstOrDefaultAsync(cancellationToken);

            
            if (clientEntity == null)
            {
                _logger.LogDebug("No task received with id: [{TaskId}]", request.TaskId);
                return response;
            }
            
            _logger.LogDebug("Successfully task received with id: [{TaskId}]", request.TaskId);

            response = ExtTaskToExtTaskResponseDtoMapper.Map(clientEntity);
            
            return response;
            
        }
    }
}
