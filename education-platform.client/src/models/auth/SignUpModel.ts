import { makeObservable, observable } from 'mobx';
import StringValidator from '../../helpers/validation/StringValidator';
import FileValidator from '../../helpers/validation/FileValidator';
import ValidationError from '../../helpers/validation/ValidationError';

export default class SignUpModel {
    public email: string;
    public userName: string;
    public password: string;
    public userImage?: File;

    constructor(email: string, userName: string, password: string, userImage?: File) {
        makeObservable(this, {
            email: observable,
            password: observable,
            userName: observable,
            userImage: observable,
        });

        this.email = email;
        this.password = password;
        this.userName = userName;
        this.userImage = userImage;
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
        passwordValidator.isStrongPassword();
        return passwordValidator.errors;
    }

    validateUserName(): ValidationError[] {
        const userNameValidator = new StringValidator(this.userName);
        userNameValidator.required();
        userNameValidator.isUserName();
        return userNameValidator.errors;
    }

    validateUserImage(): ValidationError[] {
        const userImageValidator = new FileValidator(this.userImage);
        userImageValidator.isImage();
        userImageValidator.isSizeLessThan(1);
        return userImageValidator.errors;
    }
}
