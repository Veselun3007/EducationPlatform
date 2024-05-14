import BaseValidator from './BaseValidator';
import ValidationError from './ValidationError';

export default class DateValidator extends BaseValidator<Date | undefined> {
    constructor(value?: Date) {
        super(value);
    }

    greaterThan(value: number): void {
        if (this._value && this._value.getTime() < value) {
            this.errors.push(
                new ValidationError('validation.greaterThan', {
                    value: new Date(value).toDateString(),
                }),
            );
        }
    }
}
