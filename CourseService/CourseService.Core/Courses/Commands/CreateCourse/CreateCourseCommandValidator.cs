using FluentValidation;

namespace CourseService.Application.Courses.Commands.CreateCourse {
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand> {
        public CreateCourseCommandValidator() {
            //RuleFor(x => x.CourseName).NotEmpty().MaximumLength(128).WithMessage("Invalid course name length");
            //RuleFor(x => x.CourseDescription).MaximumLength(255).WithMessage("Invalid course description length");
            RuleFor(x => x.CourseName).NotEmpty().MaximumLength(10).WithMessage("Invalid course name length");
            RuleFor(x => x.CourseDescription).MaximumLength(255).WithMessage("Invalid course description length");
        }
    }
}
