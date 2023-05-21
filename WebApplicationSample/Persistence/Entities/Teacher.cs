using WebApplicationSample.Domain;

namespace WebApplicationSample.Persistence.Entities;

public class Teacher : Entity<int>
{
    public Teacher(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Courses = new List<Course>();
    }

    public Teacher(int id, string firstName, string lastName, DateTime birthDate) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Courses = new List<Course>();
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime BirthDate { get; private set; }
    public virtual ICollection<Course> Courses { get; private set; } 
}