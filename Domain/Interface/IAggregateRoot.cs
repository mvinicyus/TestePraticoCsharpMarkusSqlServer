namespace Domain.Interface
{
    public interface IAggregateRoot
    {
    }
    public interface IAggregateRoot<out TKey> : IHaveId<TKey>
    {
    }
}
