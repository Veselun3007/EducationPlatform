using Microsoft.AspNetCore.Http;
using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Commands.UpdateStudentAssignment {
    public class UpdateStudentAssignmentCommand : ICommand<object> {
        public UpdateStudentAssignmentCommand() { }

        public UpdateStudentAssignmentCommand(List<IFormFile>? assignmentFiles, int studentAssignmentId, string userId) {
            AssignmentFiles = assignmentFiles;
            StudentAssignmentId = studentAssignmentId;
            UserId = userId;
        }

        public List<IFormFile>? AssignmentFiles { get; set; }
        public int StudentAssignmentId { get; set; }
        public string UserId { get; set; }
    }
}
