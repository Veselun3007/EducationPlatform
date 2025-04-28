namespace Chat.Domain.Interfaces
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
