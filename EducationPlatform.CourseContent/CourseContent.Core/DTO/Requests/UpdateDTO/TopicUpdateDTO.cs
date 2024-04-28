using CourseContent.Domain.Entities;

namespace CourseContent.Core.DTO.Requests.UpdateDTO
{
    public class TopicUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public required string Title { get; set; }

        public static Topic FromTopicUpdateDto(TopicUpdateDTO topic)
        {
            return new Topic
            {
                Id = topic.Id,
                CourseId = topic.CourseId,
                Title = topic.Title
            };
        }
    }
}
