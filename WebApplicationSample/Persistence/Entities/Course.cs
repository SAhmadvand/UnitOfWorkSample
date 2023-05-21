using WebApplicationSample.Domain;

namespace WebApplicationSample.Persistence.Entities;

public class Course : Entity<int>
{
    public Course(string title, byte unit)
    {
        Title = title;
        Unit = unit;
    }

    public Course(int id, string title, byte unit) : base(id)
    {
        Title = title;
        Unit = unit;
    }

    public string Title { get; private set; }
    public byte Unit { get; private set; }
}