using CourseService.Domain.Enums;

namespace CourseService.Application.DTO.Response
{
    public class CourseUserOutDTO
    {
        public int CourseUserId { get; set; }
        public Roles Role { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? UserImage { get; set; }
    }
}
