using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess.Models
{
    public class TaskDetails
    {
        [Required]
        [Key]
        public int TaskId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime FinishDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        public int? MainTaskId { get; set; }
        public TaskDetails SubTask { get; set; }
    }
}
