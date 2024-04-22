import BaseValidator from './BaseValidator';
import ValidationError from './ValidationError';

export default class NumberValidator extends BaseValidator<number | undefined> {
    constructor(value?: number) {
        super(value);
    }

    lessThan(value: number): void {
        if (this._value && this._value > value) {
            this.errors.push(
                new ValidationError('validation.lessThan', { value: value.toString() }),
            );
        }
    }

    greaterThan(value: number): void {
        if (this._value && this._value < value) {
            this.errors.push(
                new ValidationError('validation.greaterThan', { value: value.toString() }),
            );
        }
    }
}
