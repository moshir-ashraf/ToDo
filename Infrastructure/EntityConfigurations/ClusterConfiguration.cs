using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Entity;

namespace Infrastructure.EntityConfigurations
{
    internal class ClusterConfiguration : IEntityTypeConfiguration<Cluster>
    {
        public void Configure(EntityTypeBuilder<Cluster> builder)
        {
            builder.HasIndex(c => c.Name).IsUnique();

            builder.HasMany(c => c.Tasks)
                .WithOne(t => t.Cluster)
                .HasForeignKey(t => t.ClusterId);

            builder.HasOne(c => c.User)
                .WithMany(u => u.Clusters)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
