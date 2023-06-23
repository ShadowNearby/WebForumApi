namespace WebForumApi.Domain.Entities.Common;

public abstract class Entity<T>
{
    public virtual T Id { get; set; } = default!;
}
