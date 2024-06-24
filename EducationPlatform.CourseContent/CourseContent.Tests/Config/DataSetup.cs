using CourseContent.Domain.Entities;
using CourseContent.Infrastructure.Context;

internal class DataSetup(EducationPlatformContext dbContext)
{
    private readonly EducationPlatformContext _dbContext = dbContext;

    public void AddBaseData()
    {
        _dbContext.Database.EnsureCreated();
        AddCourses();
        AddTopics();
        AddAssignments();
        AddMaterials();
        _dbContext.SaveChangesAsync();
    }

    private void AddCourses()
    {
        var courses = new List<Course>
        {
            new() { Id = 1, CourseName = "Test Course 1", CourseDescription = "Test Description 1", CourseLink = "" },
            new() { Id = 2, CourseName = "Test Course 2", CourseDescription = "Test Description 2", CourseLink = "" },
        };
        _dbContext.Courses.AddRange(courses);
    }

    private void AddTopics()
    {
        var topics = new List<Topic>
        {
            new () { CourseId = 1, Title = "Topic 1" },
            new () { CourseId = 1, Title = "Topic 2" },
            new () { CourseId = 1, Title = "Topic 3" }
        };
        _dbContext.Topics.AddRange(topics);
    }

    private void AddAssignments()
    {
        var assignments = new List<Assignment>
        {
            new() {
                CourseId = 1,
                TopicId = null,
                AssignmentName = "New task 1",
                AssignmentDescription = "Task description 1",
                AssignmentDatePublication = DateTime.UtcNow,
                AssignmentDeadline =DateTime.UtcNow.AddDays(7),
                MaxMark = 10,
                MinMark = 5,
                IsRequired = true,
                IsEdited = true,
                EditedTime = DateTime.UtcNow
            },
            new() {
                CourseId = 1,
                TopicId = null,
                AssignmentName = "New task 2",
                AssignmentDescription = "Task description 2",
                AssignmentDatePublication = DateTime.UtcNow,
                AssignmentDeadline = DateTime.UtcNow.AddDays(7),
                MaxMark = 10,
                MinMark = 5,
                IsRequired = true,
                IsEdited = true,
                EditedTime = DateTime.UtcNow
            },
            new() {
                CourseId = 1,
                TopicId = null,
                AssignmentName = "New task 3",
                AssignmentDescription = "Task description 2",
                AssignmentDatePublication = DateTime.UtcNow,
                AssignmentDeadline = DateTime.UtcNow.AddDays(7),
                MaxMark = 10,
                MinMark = 5,
                IsRequired = true,
                IsEdited = true,
                EditedTime = DateTime.UtcNow
            },
            new() {
                CourseId = 1,
                TopicId = null,
                AssignmentName = "New task 4",
                AssignmentDescription = "Task description 2",
                AssignmentDatePublication = DateTime.UtcNow,
                AssignmentDeadline = DateTime.UtcNow.AddDays(7),
                MaxMark = 10,
                MinMark = 5,
                IsRequired = true,
                IsEdited = true,
                EditedTime = DateTime.UtcNow
            }
        };
        _dbContext.Assignments.AddRange(assignments);
    }

    private void AddMaterials()
    {
        var materials = new List<Material>
        {
            new ()
            {
                CourseId = 1,
                TopicId = null,
                MaterialName = "Material 1",
                MaterialDescription = "Material description 1",
                MaterialDatePublication = DateTime.UtcNow
            },
            new ()
            {
                CourseId = 1,
                TopicId = null,
                MaterialName = "Material 2",
                MaterialDescription = "Material description 2",
                MaterialDatePublication = DateTime.UtcNow
            },
            new ()
            {
                CourseId = 1,
                TopicId = null,
                MaterialName = "Material 3",
                MaterialDescription = "Material description 3",
                MaterialDatePublication = DateTime.UtcNow
            },
            new ()
            {
                CourseId = 1,
                TopicId = null,
                MaterialName = "Material 4",
                MaterialDescription = "Material description 4",
                MaterialDatePublication = DateTime.UtcNow
            }
        };
        _dbContext.Materials.AddRange(materials);
    }
}