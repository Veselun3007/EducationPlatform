using System.Reflection;

namespace CourseService.Application {
    public static class AssemblyReference {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
