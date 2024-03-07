import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class LoginModel {
    public email: string;
    public password: string;

    constructor(email: string, password: string) {
        makeObservable(this, {
            email: observable,
            password: observable,
        });

        this.email = email;
        this.password = password;
    }

    validateEmail(): ValidationError[] {
        const emailValidator = new StringValidator(this.email);
        emailValidator.required();
        emailValidator.isEmail();
        return emailValidator.errors;
    }

    validatePassword(): ValidationError[] {
        const passwordValidator = new StringValidator(this.password);
        passwordValidator.required();
        return passwordValidator.errors;
    }
}
