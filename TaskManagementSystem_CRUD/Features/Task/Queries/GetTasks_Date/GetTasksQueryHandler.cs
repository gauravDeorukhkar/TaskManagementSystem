using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Models;
using EFDataAccess;
using TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date.Mapping;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTasks_Date
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResponseDto>
    {
        private readonly ITaskDbContext _dbContext;

        public GetTasksQueryHandler(ITaskDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<GetTasksQueryResponseDto> Handle(
            GetTasksQuery request,
            CancellationToken cancellationToken)
        {
            var response = new GetTasksQueryResponseDto();

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
                response.Content.Add(task);
            }

            return response;
            
        }
    }
}