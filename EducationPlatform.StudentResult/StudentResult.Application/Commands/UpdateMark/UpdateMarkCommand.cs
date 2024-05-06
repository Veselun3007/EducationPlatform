using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Commands.UpdateMark {
    public class UpdateMarkCommand : ICommand<object> {
        public UpdateMarkCommand() { }
        public UpdateMarkCommand(int newMark, int studentAssignmentId, string userId) {
            NewMark = newMark;
            StudentAssignmentId = studentAssignmentId;
            UserId = userId;
        }

        public int NewMark { get; set; }
        public int StudentAssignmentId { get; set; }
        public string UserId { get; set; }
    }
}
