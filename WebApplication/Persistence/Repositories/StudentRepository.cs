using WebApplication.Abstraction;
using WebApplication.Persistence.Entities;

namespace WebApplication.Persistence.Repositories;

public interface IStudentRepository : IRepository<Student, int>
{
    
}

public class StudentRepository : BaseRepository<Student, int>, IStudentRepository
{
    public StudentRepository(AppContext appContext) : base(appContext)
    {
    }
}