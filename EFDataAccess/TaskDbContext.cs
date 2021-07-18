using EFDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options): base(options){ }

        public DbSet<TaskDetails> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDetails>()
                .HasOne(s => s.SubTask)
                .WithMany()
                .HasForeignKey(f => f.MainTaskId);
        }

    }
}
