import { makeObservable, observable } from 'mobx';
import ValidationError from '../../helpers/validation/ValidationError';
import StringValidator from '../../helpers/validation/StringValidator';

export default class ConfirmResetPasswordModel{
    public email: string;

    constructor(email: string) {
        makeObservable(this, {
            email: observable,
        });

        this.email = email;
    }

    validateEmail(): ValidationError[] {
        const emailValidator = new StringValidator(this.email);
        emailValidator.required();
        emailValidator.isEmail();
        return emailValidator.errors;
    }
}
