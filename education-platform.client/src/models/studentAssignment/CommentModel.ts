export default interface CommentModel {
    commentId: number;
    studentassignmentId: number;
    courseUserId: number;
    commentDate: Date;
    commentText?: string;
}
