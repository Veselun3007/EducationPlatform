import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class CreateCommentModel {
    public commentText: string;
    public studentAssignmentId: number;
    public userId?: string;

    constructor(commentText: string, studentAssignmentId: number) {
        this.commentText = commentText;
        this.studentAssignmentId = studentAssignmentId;
        makeObservable(this, {
            commentText: observable,
            studentAssignmentId: observable,
            userId: observable,
        });
    }

    validateCommentText(): ValidationError[] {
        const validator = new StringValidator(this.commentText);
        validator.required();
        validator.smallerThan(250);
        return validator.errors;
    }
}
