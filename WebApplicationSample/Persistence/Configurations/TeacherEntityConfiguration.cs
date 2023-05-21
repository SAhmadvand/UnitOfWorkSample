using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationSample.Persistence.Entities;

namespace WebApplicationSample.Persistence.Configurations;

public class TeacherEntityConfiguration:IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");
        builder.HasKey(t => t.Id);

        builder.HasMany<Course>()
            .WithMany();
    }
}