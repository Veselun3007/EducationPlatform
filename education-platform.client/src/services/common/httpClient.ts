import axios from 'axios';
import { REFRESH_TOKEN } from './routesAPI';
import TokenResponseModel from '../../models/auth/TokenResponseModel';

const httpClient = axios.create();

// httpClient.defaults.headers.common['Authorization'] =
//     'Bearer ' + localStorage.getItem('accessToken');

async function refreshToken() {
    const accessToken = (
        await httpClient.post(REFRESH_TOKEN, {
            refreshToken: localStorage.getItem('refreshToken'),
        })
    ).data.result as string;

    localStorage.setItem('accessToken', accessToken);
}

httpClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

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
