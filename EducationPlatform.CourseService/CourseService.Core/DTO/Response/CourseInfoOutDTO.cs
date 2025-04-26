namespace CourseService.Application.DTO.Response
{
    public class CourseInfoOutDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public CourseUserOutDTO UserInfo { get; set; }
        public AdminInfoDTO AdminInfo { get; set; }
    }
}

