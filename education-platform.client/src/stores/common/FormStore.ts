import ValidationError from '../../helpers/validation/ValidationError';
import IStore from './IStore';

export default abstract class FormStore implements IStore {
    abstract data: unknown;
    abstract errors: Record<string, ValidationError | null>;
    abstract get isValid(): boolean;

    reset() {
        Object.keys(this.errors).forEach((key) => (this.errors[key] = null));
    }

    abstract submit(...params: unknown[]): void;
}
