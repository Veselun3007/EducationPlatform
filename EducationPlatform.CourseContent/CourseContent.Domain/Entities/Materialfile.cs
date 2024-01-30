using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Materialfile : IAggregateRoot
{
    public int Id { get; set; }

    public int MaterialId { get; set; }

    public string? MaterialFile { get; set; }

    public virtual Material Material { get; set; } = null!;
}
