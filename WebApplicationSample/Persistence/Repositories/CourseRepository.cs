using WebApplicationSample.Abstraction;
using WebApplicationSample.Persistence.Entities;

namespace WebApplicationSample.Persistence.Repositories;

public interface ICourseRepository : IRepository<Course, int>
{
}

public class CourseRepository : BaseRepository<Course, int>, ICourseRepository
{
    public CourseRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}