namespace CourseContent.Domain.Interfaces
{
    public interface IAggregateRoot<TKey>
    {
        public TKey Id { get; set; }
    }
}
