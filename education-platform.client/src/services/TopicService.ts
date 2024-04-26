import { AxiosError } from 'axios';
import CreateUpdateTopicModel from '../models/topic/CreateUpdateTopicModel';
import TopicModel from '../models/topic/TopicModel';
import AuthService from './AuthService';
import httpClient from './common/httpClient';
import {
    CREATE_TOPIC,
    DELETE_TOPIC,
    GET_ALL_TOPIC,
    GET_BY_ID_TOPIC,
    UPDATE_TOPIC,
} from './common/routesAPI';
import LoginRequiredError from '../errors/LoginRequiredError';
import ServiceError from '../errors/ServiceError';

export default class TopicService {
    private readonly _authService: AuthService;

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    async createTopic(topic: CreateUpdateTopicModel) {
        try {
            const createdTopic = (await httpClient.postForm(CREATE_TOPIC, topic)).data
                .result as TopicModel;

            return createdTopic;
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

    async updateTopic(id: number, topic: CreateUpdateTopicModel) {
        try {
            const updatedTopic = (await httpClient.put(UPDATE_TOPIC + id, topic)).data
                .result as TopicModel;

            return updatedTopic;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.topicNotFound');
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

    async deleteTopic(id: number) {
        try {
            await httpClient.delete(DELETE_TOPIC + id);
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.topicNotFound');
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

    async getTopicById(id: number) {
        try {
            const topic = (await httpClient.get(GET_BY_ID_TOPIC + id)).data
                .result as TopicModel;

            return topic;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.topicNotFound');
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

    async getTopics(courseId: number) {
        try {
            const topics = (await httpClient.get(GET_ALL_TOPIC + courseId)).data
                .result as TopicModel[];

            return topics;
        } catch (error) {
            if (error instanceof AxiosError) {
                if (error.response) {
                    switch (error.response.status) {
                        case 404:
                            throw new ServiceError('glossary.topicNotFound');
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
