import FileValidator from '../../helpers/validation/FileValidator';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class CreateUpdateModelModel {
    courseId: number;
    topicId?: number;
    materialName: string;
    materialDescription?: string;
    materialDatePublication: Date;
    materialFiles?: File[];
    materialLinks?: string[];

    constructor(
        courseId: number,
        materialName: string,
        materialDatePublication: Date,
        topicId?: number,
        materialDescription?: string,
        materialFiles?: File[],
        materialLinks?: string[],
    ) {
        this.courseId = courseId;
        this.topicId = topicId;
        this.materialName = materialName;
        this.materialDescription = materialDescription;
        this.materialDatePublication = materialDatePublication;
        this.materialFiles = materialFiles;
        this.materialLinks = materialLinks;
    }

    validateMaterialName(): ValidationError[] {
        const validator = new StringValidator(this.materialName);
        validator.required();
        validator.smallerThan(255);

        return validator.errors;
    }

    validateMaterialFiles(): ValidationError[] {
        if (this.materialFiles) {
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

                if (validator.errors.length > 0) {
                    return validator.errors;
                }
            });
        }
        return [];
    }

    validateMaterialLinks(): ValidationError[] {
        if (this.materialLinks) {
            this.materialLinks.forEach((link) => {
                const validator = new StringValidator(link);

                validator.isLink();

                if (validator.errors.length > 0) {
                    return validator.errors;
                }
            });
        }
        return [];
    }
}
