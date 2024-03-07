export default class ServiceError extends Error {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    constructor(message = '', ...args: any[]) {
        super(message, ...args);
        this.name = this.constructor.name;
    }
}
