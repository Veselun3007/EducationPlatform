import { makeObservable, observable } from "mobx";
import ValidationError from "../../helpers/validation/ValidationError";
import FileValidator from "../../helpers/validation/FileValidator";

export default class UpdateWorkModel{
    public assignmentFiles: File[];
    public studentAssignmentId: number;
    public userId?: string;

    constructor(assignmentFiles: File[], studentAssignmentId: number) {
        this.assignmentFiles = assignmentFiles;
        this.studentAssignmentId = studentAssignmentId;
        makeObservable(this,{
            assignmentFiles: observable,
            studentAssignmentId: observable,
            userId: observable
        });
    }

    validateAssignmentFiles(): ValidationError[] {
        const errors: ValidationError[] = [];
        if (this.assignmentFiles.length !== 0) {
            this.assignmentFiles.forEach((file) => {
                const validator = new FileValidator(file);

                validator.validateFileExtension([
                    'png',
                    'jpg',
                    'jpeg',
                    'doc',
                    'pdf',
                    'docx',
                    'pptx',
                    'ppt',
                    'xls',
                    'xlsx',
                ]);

                errors.push(...validator.errors);
            });
        }
        return errors;
    }
}