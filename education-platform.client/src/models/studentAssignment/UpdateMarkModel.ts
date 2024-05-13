import { makeObservable, observable } from "mobx";
import ValidationError from "../../helpers/validation/ValidationError";
import NumberValidator from "../../helpers/validation/NumberValidator";

export default class UpdateMarkModel {
    public newMark: number;
    public studentAssignmentId: number;
    public userId?: string;

    constructor(newMark: number, studentAssignmentId: number ) {
        this.newMark = newMark;
        this.studentAssignmentId = studentAssignmentId;
        makeObservable(this,{
            newMark: observable,
            studentAssignmentId: observable,
            userId: observable,
        });
    }

    validateNewMark(maxMark: number, minMark: number): ValidationError[]{
        const validator = new NumberValidator(this.newMark);
        validator.required();
        validator.greaterThan(minMark);
        validator.lessThan(maxMark)

        return validator.errors;
    }
}