using CourseService.Application.DTO.Response;
using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;

namespace CourseService.Application.Mappings
{
    public class SpecificMapper
    {
        private readonly IAwsFileService _fileService;

        public SpecificMapper(IAwsFileService fileService)
        {
            _fileService = fileService;
        }

        private async Task<AdminInfoDTO> FromUser(User admin)
        {
            string imageLink = await SetImage(admin.UserImage);

            return new AdminInfoDTO
            {
                AdminName = admin.UserName,
                ImageLink = imageLink,
            };
        }

        private async Task<string> SetImage(string link)
        {
            string imageLink = string.Empty;

            if(!string.IsNullOrEmpty(link))
            {
                imageLink = await _fileService.GetFileLink(link) ?? string.Empty;
            }

            return imageLink;
        }

        internal async Task<CourseInfoOutDTO> From(Course course, Courseuser courseUser, User admin)
        {
            return new CourseInfoOutDTO
            {
                CourseId = course.Id,
                Title = course.CourseName,
                UserInfo = NativeMapper.FromCourseuser(courseUser),
                AdminInfo = await FromUser(admin)
            };
        }

        internal async Task<CourseUserOutDTO> FromCourseuser(Courseuser courseuser)
        {
            string imageLink = await SetImage(courseuser.User?.UserImage);

            return new CourseUserOutDTO
            {
                CourseUserId = courseuser.Id,
                Role = courseuser.Role,
                UserId = courseuser.UserId,
                UserName = courseuser.User.UserName,
                UserEmail = courseuser.User.UserEmail,
                UserImage = imageLink
            };
        }
    }
}
