using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasks.Domain.Developers.Entities;

namespace Tasks.Ifrastructure.Mapping.Developers
{
    public class DeveloperProjectMap : IEntityTypeConfiguration<DeveloperProject>
    {
        public void Configure(EntityTypeBuilder<DeveloperProject> builder)
        {
            builder.ToTable("DeveloperProjects");

            builder.HasKey(dp => dp.Id);

            builder.HasOne(dp => dp.Project)
                .WithMany(p => p.DeveloperProjects)
                .HasForeignKey(dp => dp.ProjectId);

            builder.HasOne(dp => dp.Developer)
                .WithMany(d => d.DeveloperProjects)
                .HasForeignKey(dp => dp.DeveloperId);

            builder.HasIndex(dp => new { dp.ProjectId, dp.DeveloperId }).IsUnique();
        }
    }
}
