using WebApplication.Abstraction;
using WebApplication.Persistence.Entities;

namespace WebApplication.Persistence.Repositories;

public interface ITeacherRepository:IRepository<Teacher, int>
{
    
}

public class TeacherRepository:BaseRepository<Teacher, int>, ITeacherRepository
{
    public TeacherRepository(AppContext appContext) : base(appContext)
    {
    }
}

