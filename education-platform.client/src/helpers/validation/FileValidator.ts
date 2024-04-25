import BaseValidator from './BaseValidator';
import ValidationError from './ValidationError';

export default class FileValidator extends BaseValidator<File | undefined> {
    constructor(value?: File) {
        super(value);
    }

    isImage(): void {
        const supportedExtensions = ['png', 'jpeg', 'jpg'];
        if (
            this._value &&
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            !supportedExtensions.includes((this._value as File).name.split('.').pop()!)
        ) {
            this.errors.push(new ValidationError('validation.invalidImage'));
        }
    }

    isSizeLessThan(sizeInMb: number): void {
        if (this._value) {
            const fileSizeInMb = (this._value as File).size / 1024 / 1024;
            if (fileSizeInMb > sizeInMb) {
                this.errors.push(
                    new ValidationError('validation.bigFile', { size: sizeInMb }),
                );
            }
        }
    }

    validateFileExtension(extensions: string[]): void {
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        if (
            this._value &&
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            !extensions.includes(
                (this._value as File).name.split('.').pop()!.toLowerCase(),
            )
        ) {
            this.errors.push(
                new ValidationError('validation.unsupportedFileExtension', {
                    extensions: extensions.join(' '),
                }),
            );
        }
    }
}
