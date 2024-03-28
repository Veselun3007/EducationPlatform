using CourseContent.Domain.Interfaces;

namespace CourseContent.Domain.Entities;

public class Material : IAggregateRoot
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int? TopicId { get; set; }

    public required string MaterialName { get; set; }

    public string? MaterialDescription { get; set; }

    public required DateTime MaterialDatePublication { get; set; }

    public bool IsEdited { get; set; } = false;

    public DateTime? EditedTime { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Topic? Topic { get; set; }

    public virtual ICollection<Materialfile> Materialfiles { get; set; } = new List<Materialfile>();
}
