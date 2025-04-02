using CourseService.Domain.Entities;

namespace CourseService.Application.DTO.Response
{
    public class AdminOutDTO
    {
        public Course Course { get; set; }
        public Courseuser UserInfo { get; set; }
        public string? ImageLink { get; set; }
        public string AdminName { get; set; }
    }
}
