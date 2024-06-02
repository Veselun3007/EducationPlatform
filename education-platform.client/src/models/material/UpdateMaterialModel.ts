import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class UpdateMaterialModel {
    id: number;
    courseId: number;
    topicId?: number;
    materialName: string;
    materialDescription?: string;
    materialDatePublication?: Date;

    constructor(
        id: number,
        courseId: number,
        materialName: string,
        materialDatePublication?: Date,
        topicId?: number,
        materialDescription?: string,
    ) {
        this.id = id;
        this.courseId = courseId;
        this.topicId = topicId;
        this.materialName = materialName;
        this.materialDescription = materialDescription;
        this.materialDatePublication = materialDatePublication;

        makeObservable(this, {
            courseId: observable,
            topicId: observable,
            materialName: observable,
            materialDescription: observable,
            materialDatePublication: observable,
        });
    }

    validateMaterialName(): ValidationError[] {
        const validator = new StringValidator(this.materialName);
        validator.required();
        validator.smallerThan(255);

        return validator.errors;
    }
}
