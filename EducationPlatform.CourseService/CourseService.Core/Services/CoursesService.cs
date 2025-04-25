using CourseService.Application.DTO.Request;
using CourseService.Application.DTOs;
using CourseService.Application.Helpers;
using CourseService.Domain.Entities;
using CourseService.Domain.Enums;
using CourseService.Infrastructure.Interfaces;

namespace CourseService.Application.Services
{
    public class CoursesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileHelper _fileHelper;

        public CoursesService(IUnitOfWork unitOfWork, FileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _fileHelper = fileHelper;
        }

        public async Task<List<CourseInfo?>> GetAllCourseAsync(string userId)
        {
            var courses = await _unitOfWork.CourseRepository.FindAllAsync(
                c => c.Courseusers.Any(u => u.UserId == userId),
                c => c.Courseusers
            );

            List<CourseInfo> response = new List<CourseInfo>();

            foreach(var course in courses)
            {
                (Courseuser? courseuser, User? admin) = await GetAdminInfo(userId, course);

                CourseInfo courseInfo = await SetAdminInfo(course, courseuser, admin);
                response.Add(courseInfo);
            }
            return response;
        }

        public async Task<CourseInfo?> GetCourseAsync(string userId, int courseId)
        {

            var course = await _unitOfWork.CourseRepository
                .FindAnyAsync(c => c.Id == courseId && c.Courseusers
                .Any(cu => cu.UserId == userId));

            (Courseuser? courseuser, User? admin) = await GetAdminInfo(userId, course);
            CourseInfo courseInfo = await SetAdminInfo(course, courseuser, admin);

            return courseInfo;
        }

        private async Task<(Courseuser? courseuser, User? admin)> GetAdminInfo(string userId, Course course)
        {
            var courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == userId);
            var admin_courseuser = course.Courseusers.FirstOrDefault(cu => cu.Role == Roles.Admin);
            var admin = await _unitOfWork.UserRepository.GetByIdAsync(admin_courseuser.UserId);
            return (courseuser, admin);
        }

        public async Task DeleteCourseAsync(string userId, int courseId)
        {
            var course = await _unitOfWork.CourseRepository.FindAnyAsync(
                c => c.Id == courseId,
                c => c.Courseusers
            );
            var courseuser = course?.Courseusers.FirstOrDefault(cu => cu.UserId == userId);

            if(courseuser.Role == Roles.Admin)
            {
                await _unitOfWork.CourseRepository.DeleteAsync(course.Id);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<AdminDTO> CreateCourseAsync(CourseDTO request)
        {
            string courseLink = $"{request.CourseName[..Math.Min(30, request.CourseName.Length)]}-{Guid.NewGuid()}";

            var course = FromCourseDTO(request, courseLink);
            course = await _unitOfWork.CourseRepository.AddAsync(course);
            await _unitOfWork.CommitAsync();

            return new AdminDTO { Course = course };
        }

        public async Task<CourseInfo> UpdateCourseAsync(UpdateCourseDTO request)
        {
            var course = await _unitOfWork.CourseRepository.FindAnyAsync(
                c => c.Id == request.CourseId,
                c => c.Courseusers
            );
            (Courseuser? courseuser, User? admin) = await GetAdminInfo(request.UserId, course);

            if(admin != null && courseuser != null && admin.Id == request.UserId)
            {
                course.CourseName = request.CourseName;
                course.CourseDescription = request.CourseDescription;
                course = await _unitOfWork.CourseRepository.UpdateAsync(course.Id, course);
                await _unitOfWork.CommitAsync();
            }

            CourseInfo courseInfo = await SetAdminInfo(course, courseuser, admin);

            return courseInfo;
        }

        private async Task<CourseInfo> SetAdminInfo(Course course, Courseuser? courseuser, User? admin)
        {
            CourseInfo courseInfo = new CourseInfo(course, courseuser);

            courseInfo.AdminInfo.AdminName = admin.UserName;
            if(admin.UserImage != null)
            {

                courseInfo.AdminInfo.ImageLink = await _fileHelper.GetFileLink(admin.UserImage);
            }
            else
            {
                courseInfo.AdminInfo.ImageLink = String.Empty;
            }

            return courseInfo;
        }

        public static Course FromCourseDTO(CourseDTO courseDto, string link)
        {
            return new Course
            {
                CourseName = courseDto.CourseName,
                CourseDescription = courseDto.CourseDescription,
                CourseLink = link
            };
        }
    }
}
