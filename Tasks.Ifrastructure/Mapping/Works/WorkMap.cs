using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tasks.Domain.Works.Entities;

namespace Tasks.Ifrastructure.Mapping
{
    public class WorkMap : IEntityTypeConfiguration<Work>
    {
        public void Configure(EntityTypeBuilder<Work> builder)
        {
            builder.ToTable("Works");

            builder.HasKey(w => w.Id);
            builder.Property(w => w.StartTime).IsRequired();
            builder.Property(w => w.EndTime).IsRequired();
            builder.Property(w => w.Comment).HasMaxLength(300).IsRequired();

            builder.HasOne(w => w.DeveloperProject)
                .WithMany(dp => dp.Works)
                .HasForeignKey(w => w.DeveloperProjectId);
        }
    }
}
