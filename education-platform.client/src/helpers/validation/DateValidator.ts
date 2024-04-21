import BaseValidator from './BaseValidator';
import ValidationError from './ValidationError';

export default class DateValidator extends BaseValidator<Date | undefined> {
    constructor(value?: Date) {
        super(value);
    }

    // lessThan(value: number): void {
    //     if (this._value && this._value < value) {
    //         this.errors.push(
    //             new ValidationError('validation.lessThan', { value: value }),
    //         );
    //     }
    // }

    greaterThan(value: number): void {
        if (this._value && this._value.getUTCMilliseconds() > value) {
            this.errors.push(
                new ValidationError('validation.greaterThan', {
                    value: value.toLocaleString(),
                }),
            );
        }
    }
}
