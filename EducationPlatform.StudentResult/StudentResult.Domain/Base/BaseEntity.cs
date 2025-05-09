namespace StudentResult.Domain.Base
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
