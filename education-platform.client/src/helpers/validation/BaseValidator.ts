import ValidationError from './ValidationError';

export default class BaseValidator<T> {
    protected _value: T;
    public readonly errors: ValidationError[] = [];

    constructor(value: T) {
        this._value = value;
    }

    required(): void {
        if (this._value === null || this._value === undefined || this._value === '') {
            this.errors.push(new ValidationError('validation.isRequired'));
        }
    }
}
