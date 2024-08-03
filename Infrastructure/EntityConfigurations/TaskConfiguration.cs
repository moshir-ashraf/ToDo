using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entity;
using Task = ToDo.Domain.Entity.Task;

namespace Infrastructure.EntityConfigurations
{
    internal class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Cluster)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.ClusterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.TaskTags)
                .WithOne(tt => tt.Task)
                .HasForeignKey(tt => tt.TaskId);
        }
    }
}
