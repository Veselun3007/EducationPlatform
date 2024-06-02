export default interface StudentAssignmentModel {
    studentassignmentId: number;
    studentId: number;
    assignmentId: number;
    submissionDate: Date | null;
    currentMark: number | null;
    isDone: boolean;
}
