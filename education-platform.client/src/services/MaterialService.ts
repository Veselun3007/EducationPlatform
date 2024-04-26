import { AxiosError } from 'axios';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';
import CreateUpdateMaterialModel from '../models/material/CreateUpdateMaterialModel';
import MaterialModel from '../models/material/MaterialModel';
import httpClient from './common/httpClient';
import {
    CREATE_MATERIAL,
    DELETE_MATERIAL,
    GET_ALL_MATERIAL,
    GET_BY_ID_MATERIAL,
    GET_MATERIAL_FILE,
    UPDATE_MATERIAL,
} from './common/routesAPI';
import AuthService from './AuthService';

export default class MaterialService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async createMaterial(material: CreateUpdateMaterialModel) {
        try {
            const createdMaterial = (await httpClient.postForm(CREATE_MATERIAL, material))
                .data.result as MaterialModel;

            return createdMaterial;
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

    async updateMaterial(id: number, material: CreateUpdateMaterialModel) {
        try {
            const updatedMaterial = (
                await httpClient.putForm(UPDATE_MATERIAL + id, material)
            ).data.result as MaterialModel;

            return updatedMaterial;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.materialNotFound');
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

    async deleteMaterial(id: number) {
        try {
            await httpClient.delete(DELETE_MATERIAL + id);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.materialNotFound');
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

    async getMaterialById(id: number) {
        try {
            const material = (await httpClient.get(GET_BY_ID_MATERIAL + id)).data
                .result as MaterialModel;

            return material;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.materialNotFound');
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

    async getMaterials(courseId: number) {
        try {
            const materials = (await httpClient.get(GET_ALL_MATERIAL + courseId)).data
                .result as MaterialModel[];

            return materials;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.materialNotFound');
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
    async getMaterialFileById(id: number) {
        try {
            const file = (await httpClient.get(GET_MATERIAL_FILE + id)).data
                .result as string;

            return file;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.materialFileNotFound');
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
