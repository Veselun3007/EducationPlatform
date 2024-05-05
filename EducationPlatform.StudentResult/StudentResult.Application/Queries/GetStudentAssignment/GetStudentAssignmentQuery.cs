using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Queries.GetStudentAssignment {
    public class GetStudentAssignmentQuery : IQuery<object> {
        public GetStudentAssignmentQuery() { }
        public GetStudentAssignmentQuery(int assignmentId, string userId) {
            AssignmentId = assignmentId;
            UserId = userId;
        }

        public int AssignmentId { get; set; }
        public string UserId { get; set; }
    }
}
