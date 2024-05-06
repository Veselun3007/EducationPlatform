using System.Text.Json.Serialization;

namespace StudentResult.Domain.Entities;

public partial class AttachedFile {
    public int AttachedFileId { get; set; }

    [JsonIgnore]
    public int StudentassignmentId { get; set; }

    public string? AttachedFileName { get; set; }

    [JsonIgnore]
    public virtual StudentAssignment Studentassignment { get; set; } = null!;
}
