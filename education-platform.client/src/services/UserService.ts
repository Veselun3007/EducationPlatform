import { AxiosError } from "axios";
import ServiceError from "../errors/ServiceError";
import UserUpdateModel from "../models/user/UserUpdateModel";
import AuthService from "./AuthService";
import httpClient from "./common/httpClient";
import { DELETE_USER, UPDATE_USER } from "./common/routesAPI";
import UserModel from "../models/user/UserModel";
import LoginRequiredError from "../errors/LoginRequiredError";

export default class UserService {
    private readonly _authService: AuthService

    constructor(authService: AuthService) {
        this._authService = authService
    }

    async updateUser(user: UserUpdateModel): Promise<UserModel> {
        try {
            const userId = this._authService.UserId
            const updatedUser = (await httpClient.put(UPDATE_USER + userId, user)).data.result as UserModel;

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
            const userId = this._authService.UserId
            await httpClient.delete(DELETE_USER + userId);
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