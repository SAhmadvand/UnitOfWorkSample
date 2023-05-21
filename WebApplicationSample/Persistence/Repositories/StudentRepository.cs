using WebApplicationSample.Abstraction;
using WebApplicationSample.Persistence.Entities;

namespace WebApplicationSample.Persistence.Repositories;

public interface IStudentRepository : IRepository<Student, int>
{
    
}

public class StudentRepository : BaseRepository<Student, int>, IStudentRepository
{
    public StudentRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}