import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class ResetPasswordModel {
    public email: string;
    public confirmCode: string;
    public password: string;

    constructor(email: string, code: string, password: string) {
        makeObservable(this, {
            email: observable,
            confirmCode: observable,
            password: observable,
        });

        this.email = email;
        this.confirmCode = code;
        this.password = password;
    }

    validateCode(): ValidationError[] {
        const codeValidator = new StringValidator(this.confirmCode);
        codeValidator.required();
        codeValidator.isLength(6);
        return codeValidator.errors;
    }

    validatePassword(): ValidationError[] {
        const passwordValidator = new StringValidator(this.password);
        passwordValidator.required();
        passwordValidator.isStrongPassword();
        return passwordValidator.errors;
    }
}
