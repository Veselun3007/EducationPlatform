using Chat.Domain.Entities;

namespace Chat.Core.DTO.Response
{
    public class MessageOutDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; }

        public bool? IsEdit { get; set; }

        public DateTime? EditedIn { get; set; }

        public ICollection<MessageMediaOutDTO>? AttachedFiles { get; set; }
    }
}
