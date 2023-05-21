namespace WebApplicationSample.Domain;

public abstract class Entity<TKey> where TKey : struct, IEquatable<TKey>
{
    protected Entity() { }

    protected Entity(TKey id)
    {
        if (id.Equals(default))
        {
            throw new ArgumentException("Cannot assign default value to Id");
        }

        Id = id;
    }

    public TKey Id { get; protected set; }
}