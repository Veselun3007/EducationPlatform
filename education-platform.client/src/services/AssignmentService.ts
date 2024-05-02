import { AxiosError, toFormData } from 'axios';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import CreateAssignmentModel from '../models/assignment/CreateAssignmentModel';
import AuthService from './AuthService';
import httpClient from './common/httpClient';
import {
    ADD_ASSIGNMENT_FILE,
    ADD_ASSIGNMENT_LINK,
    CREATE_ASSIGNMENT,
    DELETE_ASSIGNMENT,
    DELETE_ASSIGNMENT_FILE,
    DELETE_ASSIGNMENT_LINK,
    GET_ALL_ASSIGNMENT,
    GET_ASSIGNMENT_FILE,
    GET_BY_ID_ASSIGNMENT,
    UPDATE_ASSIGNMENT,
} from './common/routesAPI';
import AssignmentModel from '../models/assignment/AssignmentModel';
import UpdateAssignmentModel from '../models/assignment/UpdateAssignmentModel';
import AssignmentFileModel from '../models/assignment/AssignmentFileModel';
import AssignmentLinkModel from '../models/assignment/AssignmentLinkModel';

export default class AssignmentService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async createAssignment(assignment: CreateAssignmentModel) {
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

    async updateAssignment(assignment: UpdateAssignmentModel) {
        try {
            const updatedAssignment = (
                await httpClient.putForm(UPDATE_ASSIGNMENT, assignment)
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
            const assignments = (await httpClient.get(GET_ALL_ASSIGNMENT + id)).data as AssignmentModel[];

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

    async deleteFileById(fileId: number) {
        try {
            await httpClient.delete(DELETE_ASSIGNMENT_FILE + fileId)
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.fileNotFound');
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

    async addFile(file: File, assignmentId: number) {
        try {
            const createdFile = (await httpClient.postForm(ADD_ASSIGNMENT_FILE + assignmentId, { file: file }))
                .data.result as AssignmentFileModel;

            return createdFile;
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

    async deleteLinkById(linkId: number) {
        try {
            await httpClient.delete(DELETE_ASSIGNMENT_LINK + linkId);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.linkNotFound');
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

    async addLink(assignmentId: number, link: string) {
        try {
            const createdLink = (await httpClient.post(ADD_ASSIGNMENT_LINK + assignmentId, link, {
                headers: {
                    'Content-Type': 'application/json'
                }
            })).data.result as AssignmentLinkModel
            return createdLink;
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
}
