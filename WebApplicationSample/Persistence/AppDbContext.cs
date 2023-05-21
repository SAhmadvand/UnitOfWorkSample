﻿using Microsoft.EntityFrameworkCore;
using WebApplicationSample.Persistence.Configurations;

namespace WebApplicationSample.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CourseEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TeacherEntityConfiguration());
    }
}