import { action, computed, makeObservable, observable } from 'mobx';
import ResetPasswordModel from '../models/auth/ResetPasswordModel';
import AuthService from '../services/AuthService';
import RootStore from './RootStore';
import { NavigateFunction } from 'react-router-dom';
import ValidationError from '../helpers/validation/ValidationError';
import ServiceError from '../errors/ServiceError';
import { debounce } from '@mui/material';


export default class ResetPasswordStore{

  private readonly _authService: AuthService;
  private readonly _rootStore: RootStore;

  data: ResetPasswordModel = new ResetPasswordModel('', '', '');
  errors: Record<string, ValidationError | null> = {
    confirmCode: null,
    password: null,
    meta: null,
  };

  constructor(rootStore: RootStore, authService: AuthService) {

    this._rootStore = rootStore;
    this._authService = authService;

    makeObservable(this, {
      data: observable,
      errors: observable,

      isValid: computed,

      setEmail: action.bound,
      onConfirmCodeChange: action.bound,
      onPasswordChange: action.bound,
      submit: action.bound,
    });
  }

  reset(): void {
    this.data.confirmCode = '';
    this.data.password = '';

    Object.keys(this.errors).forEach((keys) => (this.errors[keys] = null));
  }

  setEmail(email: string): void {
    this.data.email = email;
  }

  onConfirmCodeChange(e: React.ChangeEvent<HTMLInputElement>): void {
    this.data.confirmCode = e.target.value;

    debounce(
      action(() => {
        const codeErrors = this.data.validateCode();
        this.errors.confirmCode = codeErrors.length !== 0 ? codeErrors[0] : null;
      }),
    )();
  }

  onPasswordChange(e: React.ChangeEvent<HTMLInputElement>): void {
    this.data.password = e.target.value;

    debounce(
      action(() => {
        const passwordErrors = this.data.validatePassword();
        this.errors.password = passwordErrors.length !== 0 ? passwordErrors[0] : null;
      }),
    )();
  }

  get isValid(): boolean {
    return (
      this.data.validateCode().length === 0 &&
      this.data.validatePassword().length === 0
    );
  }

  async submit(email: string, navigate: NavigateFunction) {
    try {
      this.data.email = email;
      await this._authService.resetPassword(this.data);
      navigate('/login');
    } catch (error) {
      this.errors.meta = new ValidationError((error as ServiceError).message);
    }
  }
}
