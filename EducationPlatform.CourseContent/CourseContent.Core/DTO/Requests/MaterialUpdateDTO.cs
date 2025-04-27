namespace CourseContent.Core.DTO.Requests
{
    public class MaterialUpdateDTO : BaseUpdateDTO
    {
        public int CourseId { get; set; }

        public int? TopicId { get; set; }

        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public bool IsEdited { get; set; } = true;

        public DateTime EditedTime { get; set; } = DateTime.UtcNow;
    }
}
