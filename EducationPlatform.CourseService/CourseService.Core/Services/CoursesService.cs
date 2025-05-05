using CourseService.Application.DTO.Request;
using CourseService.Application.DTO.Response;
using CourseService.Application.Interfaces;
using CourseService.Application.Mappings;
using CourseService.Domain.Entities;
using CourseService.Domain.Enums;

namespace CourseService.Application.Services
{
    public class CoursesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SpecificMapper _mapper;
        public CoursesService(IUnitOfWork unitOfWork, SpecificMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CourseInfoOutDTO?>> GetAllCourseAsync(string userId)
        {
            var courses = await _unitOfWork.CourseRepository.FindAllAsync(
                c => c.Courseusers.Any(u => u.UserId == userId),
                c => c.Courseusers
            );
            List<CourseInfoOutDTO> response = new List<CourseInfoOutDTO>();

            foreach(var course in courses)
            {
                (Courseuser? courseuser, User? admin) = await GetAdminInfo(userId, course);
                CourseInfoOutDTO courseInfo = await _mapper.From(course, courseuser, admin);
                response.Add(courseInfo);
            }
            return response;
        }

        public async Task<CourseInfoOutDTO?> GetCourseAsync(string userId, int courseId)
        {

            var course = await _unitOfWork.CourseRepository
                .FindAnyAsync(c => c.Id == courseId && c.Courseusers
                .Any(cu => cu.UserId == userId),
                c => c.Courseusers);

            (Courseuser? courseuser, User? admin) = await GetAdminInfo(userId, course);
            CourseInfoOutDTO courseInfo = await _mapper.From(course, courseuser, admin);

            return courseInfo;
        }

        private async Task<(Courseuser? courseuser, User? admin)> GetAdminInfo(string userId, Course course)
        {
            var courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == userId);
            var adminCourseuser = course.Courseusers.FirstOrDefault(cu => cu.Role == Roles.Admin);
            var admin = await _unitOfWork.UserRepository.GetByIdAsync(adminCourseuser.UserId);
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

            var course = NativeMapper.ToCourse(request, courseLink);
            course = await _unitOfWork.CourseRepository.AddAsync(course);
            await _unitOfWork.CommitAsync();

            return new AdminDTO { Course = course };
        }

        public async Task<CourseInfoOutDTO> UpdateCourseAsync(UpdateCourseDTO request)
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

            return await _mapper.From(course, courseuser, admin);
        }
    }
}
