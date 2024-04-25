﻿using CourseService.Application.Abstractions;
using CourseService.Application.DTOs;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Context;
using CourseService.Infrastructure.Interfaces;
using FileAWS;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Application.Courses.Queries.GetAllCourses {
    public class GetAllCoursesQueryHandler : IQueryHandler<GetAllCoursesQuery, IEnumerable<CourseInfo>> {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AmazonS3 _s3;
        private readonly EducationPlatformContext _db;
        public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork, AmazonS3 s3, EducationPlatformContext db) {
            _unitOfWork = unitOfWork;
            _s3 = s3;
            _db = db;
        }

        public async Task<Result<IEnumerable<CourseInfo>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken) {
            //var courses = _unitOfWork.GetRepository<Course>().FindBy(c => c.Courseusers.Any(u => u.CourseId == c.CourseId && u.UserId == request.UserId)).ToList();
            var courses = _db.Courses.Include(c => c.Courseusers).Where(c => c.Courseusers.Any(u => u.CourseId == c.CourseId && u.UserId == request.UserId)).ToList();

            List<CourseInfo> response = new List<CourseInfo>();
      
            foreach (var course in courses) {
                CourseInfo courseInfo = new CourseInfo();             
                courseInfo.Course = course;

                User admin = await _unitOfWork.GetRepository<User>().GetByIdAsync(course.Courseusers.FirstOrDefault(cu => cu.IsAdmin == true).UserId);
                //User admin = await _unitOfWork.GetRepository<User>().GetByIdAsync((await _unitOfWork.GetRepository<Course>().GetByIdAsync(course.CourseId)).Courseusers.FirstOrDefault(cu => cu.IsAdmin == true).UserId);
                courseInfo.AdminInfo.AdminName = admin.UserName;
                if (admin.UserImage != null) {
                    try {
                        courseInfo.AdminInfo.ImageLink = await _s3.GetObjectTemporaryUrlAsync(admin.UserImage);
                    }
                    catch {
                        return Errors.CourseError.TestError();
                    }
                }
                else {
                    courseInfo.AdminInfo.ImageLink = String.Empty;
                }

                Courseuser courseuser = course.Courseusers.FirstOrDefault(cu => cu.UserId == request.UserId);
                courseInfo.UserInfo.IsAdmin = courseuser.IsAdmin;
                courseInfo.UserInfo.Role = courseuser.Role;

                response.Add(courseInfo);
            }
            return response;
        }
    }
}
