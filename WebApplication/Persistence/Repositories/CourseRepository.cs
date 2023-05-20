using WebApplication.Abstraction;
using WebApplication.Persistence.Entities;

namespace WebApplication.Persistence.Repositories;

public interface ICourseRepository : IRepository<Course, int>
{
}

public class CourseRepository : BaseRepository<Course, int>, ICourseRepository
{
    public CourseRepository(AppContext appContext) : base(appContext)
    {
    }
}