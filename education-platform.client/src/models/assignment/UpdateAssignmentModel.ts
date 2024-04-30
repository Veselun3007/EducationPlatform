import { makeObservable, observable } from 'mobx';
import BaseValidator from '../../helpers/validation/BaseValidator';
import DateValidator from '../../helpers/validation/DateValidator';
import FileValidator from '../../helpers/validation/FileValidator';
import NumberValidator from '../../helpers/validation/NumberValidator';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class UpdateAssignmentModel {
    public id: number;
    public courseId: number;
    public topicId?: number;
    public assignmentName: string;
    public assignmentDescription?: string;
    public maxMark: number;
    public minMark: number;
    public isRequired: boolean;
    public assignmentDatePublication?: Date;
    public assignmentDeadline: Date;
    public assignmentFiles: File[];
    public assignmentLinks: string[];

    constructor(
        id: number,
        courseId: number,
        assignmentName: string,
        maxMark: number,
        minMark: number,
        isRequired: boolean,
        assignmentDeadline: Date,
        assignmentFiles: File[],
        assignmentLinks: string[],
        assignmentDatePublication?: Date,
        topicId?: number,
        assignmentDescription?: string,
    ) {
        this.id = id;
        this.courseId = courseId;
        this.topicId = topicId;
        this.assignmentName = assignmentName;
        this.assignmentDescription = assignmentDescription;
        this.maxMark = maxMark;
        this.minMark = minMark;
        this.isRequired = isRequired;
        this.assignmentDatePublication = assignmentDatePublication;
        this.assignmentDeadline = assignmentDeadline;
        this.assignmentFiles = assignmentFiles;
        this.assignmentLinks = assignmentLinks;

        makeObservable(this, {
            courseId: observable,
            topicId: observable,
            assignmentName: observable,
            assignmentDescription: observable,
            minMark: observable,
            maxMark: observable,
            isRequired: observable,
            assignmentDatePublication: observable,
            assignmentDeadline: observable,
            assignmentFiles: observable,
            assignmentLinks: observable,
        });
    }

    validateAssignmentName(): ValidationError[] {
        const validator = new StringValidator(this.assignmentName);
        validator.required();
        validator.smallerThan(255);

        return validator.errors;
    }

    validateMinMark(): ValidationError[] {
        const validator = new NumberValidator(this.minMark);
        validator.required();
        validator.greaterThan(0);
        validator.lessThan(this.maxMark);
        validator.lessThan(99);
        return validator.errors;
    }

    validateMaxMark(): ValidationError[] {
        const validator = new NumberValidator(this.maxMark);
        validator.required();
        validator.greaterThan(1);
        validator.greaterThan(this.minMark);
        validator.lessThan(100);

        return validator.errors;
    }

    validateIsRequired(): ValidationError[] {
        const validator = new BaseValidator(this.isRequired);
        validator.required();

        return validator.errors;
    }

    validateAssignmentDeadline(): ValidationError[] {
        const validator = new DateValidator(this.assignmentDeadline);
        validator.required();
        validator.greaterThan(Date.now());

        return validator.errors;
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

    validateAssignmentLinks(): ValidationError[] {
        const errors: ValidationError[] = [];
        if (this.assignmentLinks.length !== 0) {
            this.assignmentLinks.forEach((link) => {
                const validator = new StringValidator(link);

                validator.isLink();

                errors.push(...validator.errors);
            });
        }
        return errors;
    }
}
