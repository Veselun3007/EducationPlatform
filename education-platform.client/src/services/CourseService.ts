import { AxiosError } from 'axios';
import CreateCourseModel from '../models/course/CreateCourseModel';
import AuthService from './AuthService';
import httpClient from './common/httpClient';
import {
    CREATE_COURSE,
    DELETE_COURSE,
    GET_ALL_COURSE,
    GET_COURSE,
    UPDATE_COURSE,
} from './common/routesAPI';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import UpdateCourseModel from '../models/course/UpdateCourseModel';
import DeleteCourseModel from '../models/course/DeleteCourseModel';
import CourseInfoModel from '../models/course/CourseInfoModel';

export default class CourseService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async getCourses() {
        try {
            const courses = (await httpClient.get(GET_ALL_COURSE))
                .data as CourseInfoModel[];

            return courses;
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

    async getCourse(id: number) {
        try {
            const course = (await httpClient.get(GET_COURSE + '?courseId=' + id))
                .data as CourseInfoModel;

            return course;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 404:
                            throw new ServiceError('glossary.courseNotFound');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async createCourse(course: CreateCourseModel) {
        try {
            const createdCourse = (await httpClient.post(CREATE_COURSE, course))
                .data as CourseInfoModel;

            return createdCourse;
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

    async updateCourse(course: UpdateCourseModel) {
        try {
            course.userId = this._authService.UserId;
            const updatedCourse = (await httpClient.put(UPDATE_COURSE, course))
                .data as CourseInfoModel;

            return updatedCourse;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 404:
                            throw new ServiceError('glossary.courseNotFound');
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

    async deleteCourse(courseId: number) {
        try {
            const course = new DeleteCourseModel(courseId, this._authService.UserId);
            await httpClient.delete(DELETE_COURSE, { data: course });
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.courseNotFound');
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
