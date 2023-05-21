using WebApplicationSample.Abstraction;
using WebApplicationSample.Persistence.Entities;

namespace WebApplicationSample.Persistence.Repositories;

public interface ITeacherRepository:IRepository<Teacher, int>
{
    
}

public class TeacherRepository:BaseRepository<Teacher, int>, ITeacherRepository
{
    public TeacherRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}

