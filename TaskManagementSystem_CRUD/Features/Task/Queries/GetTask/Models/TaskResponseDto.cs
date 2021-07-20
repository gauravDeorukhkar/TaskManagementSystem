using System;

namespace TaskManagementSystem_CRUD.Features.Task.Queries.GetTask.Models
{
    public class TaskResponseDto
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string State { get; set; }  // Would make this enum
        public int MainTaskId { get; set; } // return MainTask Name
    }
}