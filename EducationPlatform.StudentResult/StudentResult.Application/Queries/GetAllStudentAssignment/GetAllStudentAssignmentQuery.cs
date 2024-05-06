using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Queries.GetAllStudentAssignment {
    public class GetAllStudentAssignmentQuery : IQuery<object> {
        public GetAllStudentAssignmentQuery() { }
        public GetAllStudentAssignmentQuery(int assignmentId, string userId) {
            AssignmentId = assignmentId;
            UserId = userId;
        }

        public int AssignmentId { get; set; }
        public string UserId { get; set; }
    }
}
