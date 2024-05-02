import { AxiosError } from "axios";
import AuthService from "./AuthService";
import httpClient from "./common/httpClient";
import { CREATE_COURSE_USER, DELETE_COURSE_USER, GET_COURSE_USERS, UPDATE_COURSE_USER } from "./common/routesAPI";
import LoginRequiredError from "../errors/LoginRequiredError";
import ServiceError from "../errors/ServiceError";
import CourseInfoModel from "../models/course/CourseInfoModel";
import CourseUserInfoModel from "../models/courseUser/CourseUserInfoModel";
import CreateCourseUserModel from "../models/courseUser/CreateCourseUserModel";
import UpdateCourseUserModel from "../models/courseUser/UpdateCourseUserModel";
import DeleteCourseUserModel from "../models/courseUser/DeleteCourseUserModel";

export default class CourseUserService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async getUsers(courseId: number) {
        try {
            const users = (await httpClient.get(GET_COURSE_USERS+'?courseId='+courseId))
                .data as CourseUserInfoModel[];

            return users;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
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

    async createUser(courseLink: string) {
        try {
            await httpClient.post(CREATE_COURSE_USER, new CreateCourseUserModel(courseLink, this._authService.UserId))
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 409:
                            throw new ServiceError('glossary.alreadyJoinedCourse');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async updateCourseUser(user: UpdateCourseUserModel) {
        try {
            user.userId = this._authService.UserId;
            const updatedUser = (await httpClient.put(UPDATE_COURSE_USER, user))
                .data as CourseUserInfoModel;

            return updatedUser;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 404:
                            throw new ServiceError('glossary.userNotFound');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async deleteCourseUser(user: DeleteCourseUserModel) {
        try {
            user.userId = this._authService.UserId;
            await httpClient.delete(DELETE_COURSE_USER, { data: user });
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 404:
                            throw new ServiceError('glossary.userNotFound');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }
}