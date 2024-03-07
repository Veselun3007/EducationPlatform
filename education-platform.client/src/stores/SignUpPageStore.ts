import { makeObservable, observable, action, computed, flow } from 'mobx';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import SignUpModel from '../models/auth/SignUpModel';
import debounce from '../helpers/debounce';
import FormStore from './common/FormStore';
import ValidationError from '../helpers/validation/ValidationError';
import { NavigateFunction } from 'react-router-dom';
import ServiceError from '../errors/ServiceError';

export default class SignUpPageStore extends FormStore {
    private readonly _authService: AuthService;
    private readonly _rootStore: RootStore;

    data: SignUpModel = new SignUpModel('', '', '');
    errors: Record<string, ValidationError | null> = {
        email: null,
        userName: null,
        password: null,
        userImage: null,
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
            onUserNameChange: action.bound,
            onUserImageChange: action.bound,
            submit: flow.bound,
        });
    }

    reset(): void {
        this.data.email = '';
        this.data.userName = '';
        this.data.password = '';
        this.data.userImage = undefined;

        super.reset();
    }

    get isValid(): boolean {
        return (
            this.data.validateEmail().length === 0 &&
            this.data.validateUserImage().length === 0 &&
            this.data.validateUserName().length === 0 &&
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

    onUserNameChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.userName = e.target.value;

        debounce(
            action(() => {
                const userNameErrors = this.data.validateUserName();
                this.errors.userName =
                    userNameErrors.length !== 0 ? userNameErrors[0] : null;
            }),
        )();
    }

    onUserImageChange(e: React.ChangeEvent<HTMLInputElement>): void {
        if (e.target.files) {
            this.data.userImage = e.target.files[0];
        }

        debounce(
            action(() => {
                const userImageErrors = this.data.validateUserImage();
                this.errors.userImage =
                    userImageErrors.length !== 0 ? userImageErrors[0] : null;
            }),
        )();
    }

    *submit(navigate: NavigateFunction) {
        try {
            yield this._authService.signUp(this.data);
            navigate(`/confirmEmail/${this.data.email}`);
        } catch (error) {
            this.errors.meta = new ValidationError((error as ServiceError).message);
        }
    }
}
