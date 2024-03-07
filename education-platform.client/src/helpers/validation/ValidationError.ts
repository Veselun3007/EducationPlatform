export default class ValidationError {
    errorKey: string;
    options?: Record<string, unknown>;

    constructor(errorKey: string, options?: Record<string, unknown>) {
        this.errorKey = errorKey;
        this.options = options;
    }
}
