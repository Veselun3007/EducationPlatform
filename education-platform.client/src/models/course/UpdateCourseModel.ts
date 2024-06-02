import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class UpdateCourseModel {
    public courseId: number;
    public courseName: string;
    public courseDescription?: string;
    public userId?: string;

    constructor(courseId: number, name: string, description?: string) {
        makeObservable(this, {
            courseId: observable,
            courseName: observable,
            courseDescription: observable,
        });
        this.courseId = courseId;
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
