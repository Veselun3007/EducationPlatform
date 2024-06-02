import { AxiosError } from 'axios';
import SAInfoModel from '../models/studentAssignment/SAInfoModel';
import AuthService from './AuthService';
import httpClient from './common/httpClient';
import {
    CREATE_COMMENT,
    EVALUATE,
    GET_ALL_SA,
    GET_FILELINK,
    GET_SA,
    UPDATE_WORK,
} from './common/routesAPI';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import CreateCommentModel from '../models/studentAssignment/CreateComentModel';
import CommentInfoModel from '../models/studentAssignment/CommenttInfoModel';
import UpdateMarkModel from '../models/studentAssignment/UpdateMarkModel';
import UpdateWorkModel from '../models/studentAssignment/UpdateWorkModel';

export default class StudentAssignmentService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async getStudentAssignment(id: number) {
        try {
            const studentAssignment = (await httpClient.get(GET_SA + id))
                .data as SAInfoModel;

            return studentAssignment;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async getStudentAssignments(id: number) {
        try {
            const studentAssignments = (await httpClient.get(GET_ALL_SA + id))
                .data as SAInfoModel[];

            return studentAssignments;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async getFileLink(id: number) {
        try {
            const fileLink = (await httpClient.get(GET_FILELINK + id)).data as string;

            return fileLink;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async createComment(comment: CreateCommentModel) {
        try {
            comment.userId = this._authService.UserId;
            const fileLink = (await httpClient.post(CREATE_COMMENT, comment))
                .data as CommentInfoModel[];

            return fileLink;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }

    async updateMark(mark: UpdateMarkModel) {
        try {
            mark.userId = this._authService.UserId;
            const studentAssignment = (await httpClient.put(EVALUATE, mark))
                .data as SAInfoModel;

            return studentAssignment;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }
    async updateWork(work: UpdateWorkModel) {
        try {
            work.userId = this._authService.UserId;
            const studentAssignment = (await httpClient.putForm(UPDATE_WORK, work))
                .data as SAInfoModel;

            return studentAssignment;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 401:
                            this._authService.clearTokens();
                            throw new LoginRequiredError('glossary.loginToContinue');
                        case 403:
                            throw new ServiceError('glossary.noPermissions');
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
                        case 409:
                            throw new ServiceError('glossary.alreadyMarked');
                        default:
                            throw new ServiceError('glossary.somethingWentWrong');
                    }
                }
            }
            throw new ServiceError('glossary.somethingWentWrong');
        }
    }
}
