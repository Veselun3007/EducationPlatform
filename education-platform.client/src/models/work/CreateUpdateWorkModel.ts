import { makeObservable, observable } from 'mobx';
import FileValidator from '../../helpers/validation/FileValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class CreateUpdateWorkModel {
    workFiles: File[];

    constructor(workFiles: File[]) {
        this.workFiles = workFiles;

        makeObservable(this, {
            workFiles: observable,
        });
    }

    validateWorkFiles(): ValidationError[] {
        const errors: ValidationError[] = [];
        if (this.workFiles.length !== 0) {
            this.workFiles.forEach((file) => {
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
