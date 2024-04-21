/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { action, computed, flow, makeObservable, observable, runInAction } from 'mobx';
import AuthService from '../services/AuthService';
import IStore from './common/IStore';
import RootStore from './RootStore';
import UserModel from '../models/user/UserModel';
import { enqueueAlert } from '../components/Notification/NotificationProvider';
import ServiceError from '../errors/ServiceError';
import { NavigateFunction } from 'react-router-dom';
import FormStore from './common/FormStore';
import UserUpdateModel from '../models/user/UserUpdateModel';
import ValidationError from '../helpers/validation/ValidationError';
import debounce from '../helpers/debounce';
import UserService from '../services/UserService';
import LoginRequiredError from '../errors/LoginRequiredError';

export default class UserStore extends FormStore {
    private readonly _rootStore: RootStore;
    private readonly _authService: AuthService;
    private readonly _userService: UserService;

    user: UserModel | null = null; //new UserModel('renhach.valentyn@gmail.com', 'Valentyn Renhach', '/assets/Renhach.jpg')
    data: UserUpdateModel | null = null; //new UserUpdateModel(this.user.email, this.user.userName, undefined);
    errors: Record<string, ValidationError | null> = {
        email: null,
        userName: null,

        userImage: null,
        meta: null,
    };

    constructor(
        rootStore: RootStore,
        authService: AuthService,
        userService: UserService,
    ) {
        super();
        this._rootStore = rootStore;
        this._authService = authService;
        this._userService = userService;

        makeObservable(this, {
            user: observable,
            data: observable,
            errors: observable,

            isValid: computed,
            previewImage: computed,

            getUser: action.bound,
            reset: action.bound,
            signOut: flow.bound,
            onEmailChange: action.bound,
            onUserImageChange: action.bound,
            onUserNameChange: action.bound,
            submit: action.bound,
            deleteUser: flow.bound,
        });
    }

    get previewImage(): string {
        return this.data!.userImage ? URL.createObjectURL(this.data!.userImage) : '';
    }

    async getUser(navigate: NavigateFunction) {
        try {
            const user = await this._userService.getUser();
            let imageFile: File | undefined = undefined;
            if (user.userImage) {
                imageFile = await this._userService.getUserImage(user.userImage);
            }
            runInAction(() => {
                this.user = user;
                this.data = new UserUpdateModel(user.email, user.userName, imageFile);
            });
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                navigate('/');
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    *signOut(navigate: NavigateFunction) {
        try {
            yield this._authService.signOut();
            navigate('/login');
            enqueueAlert('glossary.signOutSuccess', 'success');
        } catch (error) {
            enqueueAlert((error as ServiceError).message, 'error');
        }
    }

    *deleteUser(navigate: NavigateFunction) {
        try {
            yield this._userService.deleteUser();
            navigate('/');
            enqueueAlert('glossary.deleteUserSuccess', 'success');
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                enqueueAlert((error as ServiceError).message, 'error');
            }
        }
    }

    reset(): void {
        this.data = null;

        this.user = null;

        super.reset();
    }

    get isValid(): boolean {
        return (
            this.data!.validateEmail().length === 0 &&
            this.data!.validateUserImage().length === 0 &&
            this.data!.validateUserName().length === 0
        );
    }

    onEmailChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data!.email = e.target.value;

        debounce(
            action(() => {
                const emailErrors = this.data!.validateEmail();
                this.errors.email = emailErrors.length !== 0 ? emailErrors[0] : null;
            }),
        )();
    }

    onUserNameChange(e: React.ChangeEvent<HTMLInputElement>): void {
        this.data!.userName = e.target.value;

        debounce(
            action(() => {
                const userNameErrors = this.data!.validateUserName();
                this.errors.userName =
                    userNameErrors.length !== 0 ? userNameErrors[0] : null;
            }),
        )();
    }

    onUserImageChange(e: React.ChangeEvent<HTMLInputElement>): void {
        if (e.target.files) {
            this.data!.userImage = e.target.files[0];
        }

        debounce(
            action(() => {
                const userImageErrors = this.data!.validateUserImage();
                this.errors.userImage =
                    userImageErrors.length !== 0 ? userImageErrors[0] : null;
            }),
        )();
    }

    async submit(navigate: NavigateFunction) {
        try {
            const updatedUser = await this._userService.updateUser(this.data!);
            let imageFile: File | undefined = undefined;
            if (updatedUser.userImage) {
                imageFile = await this._userService.getUserImage(updatedUser.userImage);
            }

            runInAction(() => {
                this.user = updatedUser;
                this.data = new UserUpdateModel(
                    updatedUser.email,
                    updatedUser.userName,
                    imageFile,
                );
            });

            enqueueAlert('glossary.editSuccess', 'success');
        } catch (error) {
            if (error instanceof LoginRequiredError) {
                navigate('/login');
                enqueueAlert(error.message, 'error');
            } else {
                this.errors.meta = new ValidationError((error as ServiceError).message);
            }
        }
    }
}
