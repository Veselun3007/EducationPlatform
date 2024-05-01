import { makeObservable, observable } from 'mobx';
import FileValidator from '../../helpers/validation/FileValidator';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class CreateMaterialModel {
    courseId: number;
    topicId?: number;
    materialName: string;
    materialDescription?: string;
    materialDatePublication?: Date;
    materialFiles: File[];
    materialLinks: string[];

    constructor(
        courseId: number,
        materialName: string,
        materialFiles: File[],
        materialLinks: string[],
        materialDatePublication?: Date,
        topicId?: number,
        materialDescription?: string,
    ) {
        this.courseId = courseId;
        this.topicId = topicId;
        this.materialName = materialName;
        this.materialDescription = materialDescription;
        this.materialDatePublication = materialDatePublication;
        this.materialFiles = materialFiles;
        this.materialLinks = materialLinks;

        makeObservable(this, {
            courseId: observable,
            topicId: observable,
            materialName: observable,
            materialDescription: observable,
            materialDatePublication: observable,
            materialFiles: observable,
            materialLinks: observable,
        });
    }

    validateMaterialName(): ValidationError[] {
        const validator = new StringValidator(this.materialName);
        validator.required();
        validator.smallerThan(255);

        return validator.errors;
    }

    validateMaterialFiles(): ValidationError[] {
        const errors: ValidationError[] = [];
        if (this.materialFiles.length !== 0) {
            this.materialFiles.forEach((file) => {
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

    validateMaterialLinks(): ValidationError[] {
        const errors: ValidationError[] = [];
        if (this.materialLinks?.length !== 0) {
            this.materialLinks.forEach((link) => {
                const validator = new StringValidator(link);

                validator.isLink();

                errors.push(...validator.errors);
            });
        }
        return errors;
    }
}
