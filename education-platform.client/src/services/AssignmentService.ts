import { AxiosError } from 'axios';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import CreateUpdateAssignmentModel from '../models/assignment/CreateUpdateAssignmentModel';
import AuthService from './AuthService';
import httpClient from './common/httpClient';
import {
    CREATE_ASSIGNMENT,
    DELETE_ASSIGNMENT,
    GET_ALL_ASSIGNMENT,
    GET_ASSIGNMENT_FILE,
    GET_BY_ID_ASSIGNMENT,
    UPDATE_ASSIGNMENT,
} from './common/routesAPI';
import AssignmentModel from '../models/assignment/AssignmentModel';

export default class AssignmentService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async createAssignment(assignment: CreateUpdateAssignmentModel) {
        try {
            const createdAssignment = (
                await httpClient.postForm(CREATE_ASSIGNMENT, assignment)
            ).data.result as AssignmentModel;

            return createdAssignment;
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

    async updateAssignment(id: number, assignment: CreateUpdateAssignmentModel) {
        try {
            const updatedAssignment = (
                await httpClient.putForm(UPDATE_ASSIGNMENT + id, assignment)
            ).data.result as AssignmentModel;

            return updatedAssignment;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
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

    async deleteAssignment(id: number) {
        try {
            await httpClient.delete(DELETE_ASSIGNMENT + id);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
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

    async getAssignmentById(id: number) {
        try {
            const assignment = (await httpClient.get(GET_BY_ID_ASSIGNMENT + id)).data
                .result as AssignmentModel;

            return assignment;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
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

    async getAssignments(id: number) {
        try {
            const assignments = (await httpClient.get(GET_ALL_ASSIGNMENT + id)).data
                .result as AssignmentModel[];

            return assignments;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.assignmentNotFound');
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

    async getAssignmentFileById(id: number) {
        try {
            const file = (await httpClient.get(GET_ASSIGNMENT_FILE + id)).data
                .result as string;

            return file;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.assignmentFileNotFound');
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
