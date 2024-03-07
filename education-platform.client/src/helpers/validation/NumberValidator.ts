import BaseValidator from './BaseValidator';

export default class NumberValidator extends BaseValidator<number | undefined> {
    constructor(value?: number) {
        super(value);
    }

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    lessThan(value: number): void {
        throw Error('Not implemented');
    }
}
