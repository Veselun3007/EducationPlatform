using StudentResult.Application.Abstractions;

namespace StudentResult.Application.Commands.CreateComment {
    public class CreateCommentCommand : ICommand<object> {
        public CreateCommentCommand() { }

        public string CommentText { get; set; }
        public int StudentAssignmentId { get; set; }
        public string UserId { get; set; }
    }
}
