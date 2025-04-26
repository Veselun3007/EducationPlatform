namespace CourseContent.Core.DTO.Requests
{
    public class TopicUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public required string Title { get; set; }
    }
}
