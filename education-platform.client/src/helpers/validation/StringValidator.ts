import BaseValidator from './BaseValidator';
import ValidationError from './ValidationError';

export default class StringValidator extends BaseValidator<string | undefined> {
    constructor(value?: string) {
        super(value);
    }

    isEmail(): void {
        if (
            this._value &&
            !/^[\w-\\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(this._value as string)
        ) {
            this.errors.push(new ValidationError('validation.invalidEmail'));
        }
    }

    isUserName(): void {
        if (
            this._value &&
            !/^[a-zA-Zа-яА-ЯґҐєЄіІїЇ]+(?: [a-zA-Zа-яА-ЯґҐєЄіІїЇ]+)*$/.test(
                this._value as string,
            )
        ) {
            this.errors.push(new ValidationError('validation.invalidUserName'));
        }
    }

    isStrongPassword(): void {
        if (
            this._value &&
            !/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&]).{8,}$/.test(
                this._value as string,
            )
        ) {
            this.errors.push(new ValidationError('validation.weakPassword'));
        }
    }

    isLength(length: number): void {
        if (this._value && this._value.length !== length) {
            this.errors.push(
                new ValidationError('validation.notSize', { length: length }),
            );
        }
    }

    smallerThan(length: number): void {
        if (this._value && this._value.length > length) {
            this.errors.push(
                new ValidationError('validation.lessThan', { value: length }),
            );
        }
    }

    isLink(): void {
        if (
            this._value &&
            // eslint-disable-next-line no-useless-escape
            !/^(https?:\/\/)?([\w\-]+(\.[\w\-]+)+\/?|localhost|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})(:\d+)?(\/\S*)?$/i.test(
                this._value as string,
            )
        ) {
            this.errors.push(new ValidationError('validation.invalidLink'));
        }
    }
}
