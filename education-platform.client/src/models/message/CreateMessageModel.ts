import { makeObservable, observable } from "mobx";
import FileValidator from "../../helpers/validation/FileValidator";
import StringValidator from "../../helpers/validation/StringValidator";
import ValidationError from "../../helpers/validation/ValidationError";

export default class CreateMessageModel {

    public courseId: number;
    public messageText?: string;
    public creatorId: number;
    public createdIn: Date = new Date(Date.now());
    public attachedFiles: File[] = [];

    constructor(courseId: number, creatorId: number, messageText = '') {
        this.courseId = courseId;
        this.messageText = messageText;
        this.creatorId = creatorId;

        makeObservable(this, {
            courseId: observable,
            messageText: observable,
            createdIn: observable,
            creatorId: observable,
            attachedFiles: observable,
        });
    }

    validateMessage(): boolean {
        if (this.messageText === '' && this.attachedFiles.length === 0) {
            return false;
        }
        return true;
    }

    validateMessageText(): ValidationError[] {
        const validator = new StringValidator(this.messageText);
        validator.smallerThan(450);

        return validator.errors;
    }

    validateAttachedFiles(): ValidationError[] {
        const errors: ValidationError[] = [];
        if (this.attachedFiles.length !== 0) {
            this.attachedFiles.forEach((file) => {
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