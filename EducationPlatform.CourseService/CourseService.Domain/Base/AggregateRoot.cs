namespace CourseService.Domain.Base
{
    public abstract class AggregateRoot<TKey>
    {
        public TKey Id { get; set; }
    }
}
