// export default class Validatior {
//     // eslint-disable-next-line @typescript-eslint/no-explicit-any
//     static haveValue(value: any): boolean {
//         if (value === null || value === undefined || value === '') {
//             return false
//         }
//         return true;
//     }

//     static isEmail(value: string) {
//         if (/^[\w-\\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(value)) {
//             return true;
//         }
//         return false;
//     }

//     static isImage(file: File): boolean {

//         // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
//         if (['png', 'jpeg', 'jpg'].includes(file.name.split('.').pop()!)) {
//             return true;
//         }
//         return false;
//     }

//     static isSizeLessThan(file: File, sizeInMb: number): boolean {
//         const fileSizeInMb = file.size / 1024 / 1024;
//         if (fileSizeInMb <= sizeInMb) {
//             return true;
//         }
//         return false;
//     }

//     static isUserName(value: string): boolean {
//         if (/^[a-zA-Zа-яА-ЯґҐєЄіІїЇ\s]+$/.test(value)) {
//             return true;
//         }
//         return false;
//     }
// }

export default class Validator<T> {
    private _value: T;
    public readonly errors: string[] = [];

    constructor(value: T) {
        this._value = value;
    }

    private isString(): boolean {
        if (typeof this._value !== 'string') {
            this.errors.push('validation.notString');
            return false;
        }
        return true;
    }

    private isNumeric(): boolean {
        if (isNaN(this._value as number)) {
            this.errors.push('validation.notNumber');
            return false;
        }
        return true;
    }

    private isFile(): boolean {
        if (this._value instanceof File) {
            return true;
        }
        this.errors.push('validation.notFile');
        return false;
    }

    required(): Validator<T> {
        if (this._value === null || this._value === undefined || this._value === '') {
            this.errors.push('validation.isRequired');
        }
        return this;
    }

    isEmail(): Validator<T> {
        if (this.isString()) {
            if (!/^[\w-\\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(this._value as string)) {
                this.errors.push('validation.invalidEmail');
            }
        }
        return this;
    }

    isUserName(): Validator<T> {
        if (this.isString()) {
            if (
                !/^[a-zA-Zа-яА-ЯґҐєЄіІїЇ]+(?: [a-zA-Zа-яА-ЯґҐєЄіІїЇ]+)*$/.test(
                    this._value as string,
                )
            ) {
                this.errors.push('validation.invalidUserName');
            }
        }
        return this;
    }

    isStrongPassword(): Validator<T> {
        if (this.isString()) {
            if (
                !/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&]).{8,}$/.test(
                    this._value as string,
                )
            ) {
                this.errors.push('validation.weakPassword');
            }
        }
        return this;
    }

    isImage(): Validator<T> {
        if (this.isFile()) {
            const supportedExtensions = ['png', 'jpeg', 'jpg'];
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            if (
                !supportedExtensions.includes(
                    (this._value as File).name.split('.').pop()!,
                )
            ) {
                this.errors.push('validation.invalidImage');
            }
        }
        return this;
    }

    isSizeLessThan(sizeInMb: number): Validator<T> {
        if (this.isFile()) {
            const fileSizeInMb = (this._value as File).size / 1024 / 1024;
            if (fileSizeInMb > sizeInMb) {
                this.errors.push('validation.bigImage');
            }
        }
        return this;
    }
}
