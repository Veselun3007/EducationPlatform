import { AxiosError } from 'axios';
import SignUpModel from '../models/auth/SignUpModel';
import httpClient from './common/httpClient';
import { CONFIRM_USER, LOGIN, SIGNOUT, SIGNUP } from './common/routesAPI';
import ServiceError from '../errors/ServiceError';
import LoginModel from '../models/auth/LoginModel';
import TokenResponseModel from '../models/auth/TokenResponseModel';
import ConfirmUserModel from '../models/auth/ConfirmUserModel';
import SignOutModel from '../models/auth/SignOutModel';

export default class AuthService {
    async signUp(data: SignUpModel): Promise<void> {
        try {
            await httpClient.postForm(SIGNUP, data);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 409:
                            throw new ServiceError('glossary.userExists');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async login(data: LoginModel): Promise<void> {
        try {
            const tokens = (await httpClient.postForm(LOGIN, data)).data
                .result as TokenResponseModel;
            localStorage.setItem('accessToken', tokens.accessToken);
            localStorage.setItem('refreshToken', tokens.refreshToken);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.wrongLoginCredentials');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async confirmUser(data: ConfirmUserModel): Promise<void> {
        try {
            await httpClient.postForm(CONFIRM_USER, data);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 400:
                            throw new ServiceError(
                                'glossary.confiramtionCodeExpiredOrWrong',
                            );
                        case 404:
                            throw new ServiceError('glossary.wrongEmail');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async signOut(): Promise<void> {
        try {
            const data = new SignOutModel(localStorage.getItem('accessToken'));
            await httpClient.postForm(SIGNOUT, data);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this.clearTokens()
                            break;
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    get UserId(): string{
        const accessToken = localStorage.getItem('accessToken');
        if(accessToken){
            return JSON.parse(window.atob(accessToken.split('.')[1])).sub 
        }

        return '';
    }

    clearTokens(): void{
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
    }
}
