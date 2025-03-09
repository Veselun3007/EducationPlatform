using CourseContent.Domain.Entities;

namespace CourseContent.Core.Helpers
{
    internal static class MappingHelpers
    {
        public static Assignmentfile CreateAssignmentFile(int id, string fileLink)
        {
            return new()
            {
                AssignmentId = id,
                AssignmentFile = fileLink
            };
        }
        public static Assignmentlink CreateAssignmentLink(string link, int id)
        {
            return new()
            {
                AssignmentId = id,
                AssignmentLink = link
            };
        }
        public static Materialfile CreateMaterialFile(int id, string fileLink)
        {
            return new()
            {
                MaterialId = id,
                MaterialFile = fileLink
            };
        }
        public static Materiallink CreateMaterialLink(string link, int id)
        {
            return new()
            {
                MaterialId = id,
                MaterialLink = link
            };
        }
    }
}