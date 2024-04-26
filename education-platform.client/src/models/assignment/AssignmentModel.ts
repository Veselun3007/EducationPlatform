import AssignmentFileModel from './AssignmentFileModel';
import AssignmentLinkModel from './AssignmentLinkModel';

export default interface AssignmentModel {
    id: number;
    topicId?: number;
    assignmentName: string;
    assignmentDescription?: string;
    maxMark: number;
    minMark: number;
    isRequired: boolean;
    assignmentDatePublication: Date;
    assignmentDeadline: Date;
    isEdited: boolean;
    editedTime?: Date;
    assignmentfiles?: AssignmentFileModel[];
    assignmentlinks?: AssignmentLinkModel[];
}
