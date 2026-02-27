using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities.Identity;
using TaskManager.Core.Entities.Projects;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Repository.Data.Context
{
    public class TaskManagerDbContext : IdentityDbContext<ApplicationUser>
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options)
        {
        }
        
        //DBSets Zone
        public DbSet<AppTask> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerDbContext).Assembly);
        }
    }
}
