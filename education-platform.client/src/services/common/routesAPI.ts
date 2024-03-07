const AUTH_BASE_ROUTE = 'https://localhost:5000/api';

export const SIGNUP = AUTH_BASE_ROUTE + '/account/sign-up';
export const LOGIN = AUTH_BASE_ROUTE + '/account/sign-in';
export const CONFIRM_USER = AUTH_BASE_ROUTE + '/account/confirm';
export const REFRESH_TOKEN = AUTH_BASE_ROUTE + '/account/refresh';
export const SIGNOUT = AUTH_BASE_ROUTE + '/account/sign-out';

export const UPDATE_USER = AUTH_BASE_ROUTE + '/userManagement/update/'
export const DELETE_USER = AUTH_BASE_ROUTE + '/userManagement/delete/'
