namespace Chat.Domain.Entities.Base
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
