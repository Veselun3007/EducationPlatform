using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Requests
{
    public class TopicDTO
    {
        public int CourseId { get; set; }

        public required string Title { get; set; }

        public static Topic FromTopicDto(TopicDTO topic)
        {
            return new Topic
            {
                CourseId = topic.CourseId,
                Title = topic.Title
            };
        }
    }
}
