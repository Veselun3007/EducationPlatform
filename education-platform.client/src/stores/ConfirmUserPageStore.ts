import { action, computed, flow, makeObservable, observable } from 'mobx';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import debounce from '../helpers/debounce';
import FormStore from './common/FormStore';
import ConfirmUserModel from '../models/auth/ConfirmUserModel';
import ValidationError from '../helpers/validation/ValidationError';
import { NavigateFunction } from 'react-router-dom';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import ServiceError from '../errors/ServiceError';

export default class ConfirmUserPageStore extends FormStore {
    private readonly _authService: AuthService;
    private readonly _rootStore: RootStore;

    data: ConfirmUserModel = new ConfirmUserModel('', '');
    errors: Record<string, ValidationError | null> = {
        code: null,
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
            setEmail: action.bound,
            onCodeChange: action.bound,
            submit: flow.bound,
        });
    }

    reset(): void {
        //this.data.email = '';
        this.data.confirmCode = '';

        super.reset();
    }

    get isValid(): boolean {
        return this.data.validateCode().length === 0;
    }

    setEmail(email: string): void {
        this.data.email = email;
    }

    onCodeChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.confirmCode = e.target.value;

        debounce(
            action(() => {
                const codeErrors = this.data.validateCode();
                this.errors.code = codeErrors.length !== 0 ? codeErrors[0] : null;
            }),
        )();
    }

    *submit(email: string, navigate: NavigateFunction) {
        try {
            this.data.email = email;
            yield this._authService.confirmUser(this.data);
            enqueueAlert('glossary.loginToContinue');
            navigate('/login');
        } catch (error) {
            this.errors.meta = new ValidationError((error as ServiceError).message);
        }
    }
}
