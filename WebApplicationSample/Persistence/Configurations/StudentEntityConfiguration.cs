using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplicationSample.Persistence.Entities;

namespace WebApplicationSample.Persistence.Configurations;

public class StudentEntityConfiguration:IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        builder.HasKey(t => t.Id);
    }
}