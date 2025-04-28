namespace Chat.Core.DTO.Request
{
    public class MessageUpdateDTO
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string? MessageText { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedIn { get; set; }
    }
}