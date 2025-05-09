using Microsoft.AspNetCore.Http;

namespace StudentResult.Application.DTO.Request
{
    public class UpdateStudentAssignmentDto
    {
        public int AssignmentId { get; set; }

        public int StudentId { get; set; }

        public DateTime? SubmissionDate { get; set; } = DateTime.UtcNow;

        public int? CurrentMark { get; set; }

        public bool? IsDone { get; set; } = true;

        public List<IFormFile>? AssignmentFiles { get; set; }

        public int StudentAssignmentId { get; set; }
    }
}
