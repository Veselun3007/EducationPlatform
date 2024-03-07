import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class ConfirmUserModel {
    public email: string;
    public confirmCode: string;

    constructor(email: string, code: string) {
        makeObservable(this, {
            email: observable,
            confirmCode: observable,
        });

        this.email = email;
        this.confirmCode = code;
    }

    validateCode(): ValidationError[] {
        const codeValidator = new StringValidator(this.confirmCode);
        codeValidator.required();
        codeValidator.isLength(6);
        return codeValidator.errors;
    }
}
