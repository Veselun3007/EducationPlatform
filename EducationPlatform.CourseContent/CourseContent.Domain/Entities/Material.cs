using CourseContent.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CourseContent.Domain.Entities;

public class Material : IAggregateRoot
{
    public int MaterialId { get; set; }

    public int CourseId { get; set; }

    public string MaterialName { get; set; } = null!;

    public string? MaterialDescription { get; set; }

    public DateTime MaterialDatePublication { get; set; } = DateTime.Now;

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Materialfile> Materialfiles { get; set; } = new List<Materialfile>();

    [NotMapped]
    [JsonIgnore]
    public List<IFormFile>? MaterialFiles { get; set; }

}
