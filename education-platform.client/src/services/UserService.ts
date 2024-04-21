import axios, { AxiosError } from 'axios';
import ServiceError from '../errors/ServiceError';
import UserUpdateModel from '../models/user/UserUpdateModel';
import AuthService from './AuthService';
import httpClient from './common/httpClient';
import { DELETE_USER, GET_USER, UPDATE_USER } from './common/routesAPI';
import UserModel from '../models/user/UserModel';
import LoginRequiredError from '../errors/LoginRequiredError';

export default class UserService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async updateUser(user: UserUpdateModel): Promise<UserModel> {
        try {
            const updatedUser = (await httpClient.putForm(UPDATE_USER, user)).data
                .result as UserModel;

            return updatedUser;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.userNotFound');
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async deleteUser(): Promise<void> {
        try {
            await httpClient.delete(DELETE_USER);
            this._authService.clearTokens();
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.userNotFound');
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async getUserImage(src: string): Promise<File | undefined> {
        try {
            const response = await axios.get(src, { responseType: 'blob' });
            // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
            const extension = (response.headers['content-type']! as string)
                .split('/')
                .pop();

            const image = new File([response.data], `image.${extension}`);
            return image;
        } catch {
            return undefined;
        }
    }

    async getUser(): Promise<UserModel> {
        try {
            const user = (await httpClient.get(GET_USER)).data.result as UserModel;

            return user;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.userNotFound');
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }
}
