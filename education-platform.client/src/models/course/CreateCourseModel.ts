import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class CreateCourseModel {
    public courseName: string;
    public courseDescription?: string;

    constructor(name: string, description?: string) {
        makeObservable(this, {
            courseName: observable,
            courseDescription: observable,
        });

        this.courseName = name;
        this.courseDescription = description;
    }

    validateName(): ValidationError[] {
        const validator = new StringValidator(this.courseName);
        validator.required();
        validator.smallerThan(128);
        return validator.errors;
    }
}
