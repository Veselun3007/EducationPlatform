import { action, computed, flow, makeObservable, observable } from 'mobx';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import ConfirmResetPasswordModel from '../models/auth/ConfirmResetPasswordModel';
import ValidationError from '../helpers/validation/ValidationError';
import { NavigateFunction } from 'react-router-dom';
import ServiceError from '../errors/ServiceError';
import { debounce } from '@mui/material';

export default class ConfirmResetPasswordPageStore{
    private readonly _authService: AuthService;
    private readonly _rootStore: RootStore;

    data: ConfirmResetPasswordModel = new ConfirmResetPasswordModel('');
    errors: Record<string, ValidationError | null> = {
        email: null,
        meta: null,
    };

    constructor(rootStore: RootStore, authService: AuthService) {
        this._rootStore = rootStore;
        this._authService = authService;

        makeObservable(this, {
            data: observable,
            errors: observable,

            isValid: computed,

            reset: action.bound,
            onEmailChange: action.bound,
            submit: action.bound,
        });
    }
    reset(): void {
        this.data.email = '';

        Object.keys(this.errors).forEach((keys)=>(this.errors[keys]=null));
    }

    onEmailChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data.email = e.target.value;
     
        debounce(
            action(() => {
                const emailErrors = this.data.validateEmail();
                this.errors.code = emailErrors.length !== 0 ? emailErrors[0] : null;
            }),
        )();
    }

    get isValid(): boolean {
        return this.data.validateEmail().length === 0;
    }

    async submit(navigate: NavigateFunction) {
        try {
            await this._authService.confirmResetPassword(this.data);
            navigate(`/reset-password/${this.data.email}`);
        } catch (error) {
            this.errors.meta = new ValidationError((error as ServiceError).message);
        }
    }
}