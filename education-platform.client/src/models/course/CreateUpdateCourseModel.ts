import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class CreateUpdateCourseModel {
    public name: string;
    public description?: string;

    constructor(name: string, description?: string) {
        makeObservable(this, {
            name: observable,
            description: observable,
        });

        this.name = name;
        this.description = description;
    }

    validateName(): ValidationError[] {
        const emailValidator = new StringValidator(this.name);
        emailValidator.required();
        return emailValidator.errors;
    }
}
