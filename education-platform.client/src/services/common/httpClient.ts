import axios from 'axios';
import { REFRESH_TOKEN } from './routesAPI';
import TokenResponseModel from '../../models/auth/TokenResponseModel';

const httpClient = axios.create();

httpClient.defaults.headers.common['Authorization'] =
    'Bearer' + localStorage.getItem('accessToken');

async function refreshToken() {
    const tokens = (
        await httpClient.post(REFRESH_TOKEN, {
            refreshToken: localStorage.getItem('refreshToken'),
        })
    ).data.result as TokenResponseModel;

    localStorage.setItem('accessToken', tokens.accessToken);
    localStorage.setItem('refreshToken', tokens.refreshToken);
}

httpClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response) {
            if (error.response.status === 401 && !originalRequest._retry) {
                originalRequest._retry = true;
                try {
                    await refreshToken();

                    return await httpClient(originalRequest);
                } catch (error) {
                    return Promise.reject(error);
                }
            }
        }

        return Promise.reject(error);
    },
);

export default httpClient;
