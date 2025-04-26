namespace CourseContent.Core.DTO.Responses
{
    public class MaterialOutDTO
    {
        public int Id { get; set; }
        public int? TopicId { get; set; }
        public required string MaterialName { get; set; }

        public string? MaterialDescription { get; set; }

        public DateTime MaterialDatePublication { get; set; }

        public bool IsEdited { get; set; }

        public DateTime? EditedTime { get; set; }

        public ICollection<MaterialfileOutDTO>? Materialfiles { get; set; }

        public ICollection<MateriallinkOutDTO>? Materiallinks { get; set; }
    }
}
