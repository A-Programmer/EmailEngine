namespace Sadin.EmailEngine.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    protected Entity(Guid id) => Id = id;

    protected Entity()
    {
    }

    public Guid Id { get; private init; }
    public bool IsDeleted { get; private set; }
    
    /// <summary>
    /// Gets the created on date and time in UTC format.
    /// </summary>
    public DateTimeOffset CreatedOnUtc { get; private set; }

    /// <summary>
    /// Gets the creator user name.
    /// </summary>
    public string? CreatedBy { get; private set; }
    /// <summary>
    /// Gets the created on date and time in UTC format.
    /// </summary>
    public DateTimeOffset? ModifiedOnUtc { get; private set; }

    /// <summary>
    /// Gets the modifier user name.
    /// </summary>
    public string? ModifiedBy { get; private set; }

    public void Delete() => IsDeleted = true;
    public void Recover() => IsDeleted = false;

    public void SetCreatorInfo(DateTimeOffset createdOn, string? createdBy)
    {
        CreatedOnUtc = createdOn;
        CreatedBy = createdBy;
    }

    public void SetModifierInfo(DateTimeOffset modifiedOn, string? modifiedBy)
    {
        ModifiedOnUtc = modifiedOn;
        ModifiedBy = modifiedBy;
    }

    public static bool operator ==(Entity? first, Entity? second) =>
        first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);

    public bool Equals(Entity? other)
    {
        if (other is null)
            return false;
        if (other.GetType() != GetType())
            return false;
        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        if (obj is not Entity entity)
            return false;
        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}