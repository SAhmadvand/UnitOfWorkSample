using WebApplication.Domain;

namespace WebApplication.Persistence.Entities;

public class Student : Entity<int>
{
    public Student(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Courses = new List<Course>();
    }

    public Student(int id, string firstName, string lastName, DateTime birthDate) : base(id)
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