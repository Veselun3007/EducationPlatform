import axios from 'axios';
import { REFRESH_TOKEN } from './routesAPI';

const httpClient = axios.create({
    formSerializer: { indexes: null },
});


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

const isoDateFormat =
    /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)((-(\d{2}):(\d{2})|Z)?)$/;

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function isIsoDateString(value: any): boolean {
    return value && typeof value === 'string' && isoDateFormat.test(value);
}
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function convertResponseDates(body: any) {
    if (body === null || body === undefined || typeof body !== 'object') return body;

    for (const key of Object.keys(body)) {
        const value = body[key];
        if (isIsoDateString(value)) {
            body[key] = new Date(value);
        } else if (typeof value === 'object') convertResponseDates(value);
    }
}

httpClient.interceptors.response.use((response) => {
    convertResponseDates(response.data);
    return response;
});

export default httpClient;
