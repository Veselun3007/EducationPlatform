import { action, computed, flow, makeObservable, observable } from 'mobx';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import LoginModel from '../models/auth/LoginModel';
import debounce from '../helpers/debounce';
import FormStore from './common/FormStore';
import ValidationError from '../helpers/validation/ValidationError';
import { NavigateFunction } from 'react-router-dom';
import ServiceError from '../errors/ServiceError';

export default class LoginPageStore extends FormStore {
    private readonly _authService: AuthService;
    private readonly _rootStore: RootStore;

    data: LoginModel = new LoginModel('', '');
    errors: Record<string, ValidationError | null> = {
        email: null,
        password: null,
        meta: null,
    };

    constructor(rootStore: RootStore, authService: AuthService) {
        super();
        this._rootStore = rootStore;
        this._authService = authService;

        makeObservable(this, {
            data: observable,
            errors: observable,

            isValid: computed,

            reset: action.bound,
            onEmailChange: action.bound,
            onPasswordChange: action.bound,
            submit: flow.bound,
        });
    }

    reset(): void {
        this.data.email = '';
        this.data.password = '';

        super.reset();
    }

    get isValid(): boolean {
        return (
            this.data.validateEmail().length === 0 &&
            this.data.validatePassword().length === 0
        );
    }

    onEmailChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.email = e.target.value;
        debounce(
            action(() => {
                const emailErrors = this.data.validateEmail();
                this.errors.email = emailErrors.length !== 0 ? emailErrors[0] : null;
            }),
        )();
    }

    onPasswordChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.password = e.target.value;

        debounce(
            action(() => {
                const passwordErrors = this.data.validatePassword();
                this.errors.password =
                    passwordErrors.length !== 0 ? passwordErrors[0] : null;
            }),
        )();
    }

    *submit(navigate: NavigateFunction) {
        try {
            yield this._authService.login(this.data);
            navigate('/dashboard');
        } catch (error) {
            this.errors.meta = new ValidationError((error as ServiceError).message);
        }
    }
}
