namespace CourseContent.Core.DTO.Requests
{
    public class TopicUpdateDTO : BaseUpdateDTO
    {
        public int CourseId { get; set; }

        public required string Title { get; set; }
    }
}
