using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Responses
{
    public class TopicOutDTO
    {
        public int CourseId { get; set; }

        public int Id { get; set; }

        public required string Title { get; set; }

        public static TopicOutDTO FromTopic(Topic topic)
        {
            return new TopicOutDTO
            {
                CourseId = topic.CourseId,
                Id = topic.Id,
                Title = topic.Title
            };
        }
    }
}
