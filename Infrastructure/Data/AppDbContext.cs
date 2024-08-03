using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ToDo.Domain.Entity;
namespace ToDo.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ToDo.Domain.Entity.Task> Tasks { get; set; }
        public DbSet<TaskTag> TasksTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
