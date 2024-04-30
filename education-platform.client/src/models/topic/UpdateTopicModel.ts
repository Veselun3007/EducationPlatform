import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class UpdateTopicModel {
    public id?: number;
    public courseId: number;
    public title: string;

    constructor(courseId: number, title: string, id?: number) {
        this.id = id;
        this.courseId = courseId;
        this.title = title;

        makeObservable(this, {
            courseId: observable,
            title: observable,
        });
    }

    validateTitle(): ValidationError[] {
        const validator = new StringValidator(this.title);

        validator.required();
        validator.smallerThan(255);

        return validator.errors;
    }
}
