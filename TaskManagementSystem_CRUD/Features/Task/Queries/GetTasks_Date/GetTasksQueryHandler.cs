using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Models;
using EFDataAccess;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Mapping;
using Microsoft.Extensions.Logging;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResponseDto>
    {
        private readonly ITaskDbContext _dbContext;
        private readonly ILogger<GetTasksQueryHandler> _logger;

        public GetTasksQueryHandler(ITaskDbContext dbContext,
                    ILogger<GetTasksQueryHandler> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<GetTasksQueryResponseDto> Handle(
            GetTasksQuery request,
            CancellationToken cancellationToken)
        {
            var response = new GetTasksQueryResponseDto();

            _logger.LogDebug("Attempting to Fetch InProgress Tasks as of date: [{Date}]", request.Date);

            var result = await _dbContext.Tasks
                .Where(c => c.StartDate <= request.Date && c.FinishDate >= request.Date && c.State == "InProgress")
                .ToListAsync(cancellationToken);
            
            
            foreach (var entity in result)
            {
                if (entity == null)
                {
                    continue;
                }

                var task = TaskToTaskResponseDtoMapper.Map(entity);
                _logger.LogDebug("Adding Inprogress Tasks to a list as of date: [{Date}]", request.Date);

                response.Content.Add(task);
            }

            return response;
            
        }
    }
}