const AUTH_BASE_ROUTE = 'https://localhost:5000/api';

export const SIGNUP = AUTH_BASE_ROUTE + '/account/sign-up';
export const LOGIN = AUTH_BASE_ROUTE + '/account/sign-in';
export const CONFIRM_USER = AUTH_BASE_ROUTE + '/account/confirm';
export const REFRESH_TOKEN = AUTH_BASE_ROUTE + '/account/refresh';
export const SIGNOUT = AUTH_BASE_ROUTE + '/account/sign-out';

export const UPDATE_USER = AUTH_BASE_ROUTE + '/userManagement/update';
export const GET_USER = AUTH_BASE_ROUTE + '/userManagement/get';
export const DELETE_USER = AUTH_BASE_ROUTE + '/userManagement/delete';

const COURSE_CONTENT_BASE_ROUTE = 'https://localhost:5002/api';

export const CREATE_ASSIGNMENT = COURSE_CONTENT_BASE_ROUTE + '/assignment/create';
export const UPDATE_ASSIGNMENT = COURSE_CONTENT_BASE_ROUTE + '/assignment/update/';
export const DELETE_ASSIGNMENT = COURSE_CONTENT_BASE_ROUTE + '/assignment/delete';
export const GET_BY_ID_ASSIGNMENT = COURSE_CONTENT_BASE_ROUTE + '/assignment/getById/';
export const GET_ALL_ASSIGNMENT = COURSE_CONTENT_BASE_ROUTE + '/assignment/getAll/';
export const REMOVE_LIST_ASSIGNMENT =
    COURSE_CONTENT_BASE_ROUTE + '/assignment/removeList';
export const GET_ASSIGNMENT_FILE = COURSE_CONTENT_BASE_ROUTE + '/assignment/getFileById/';
export const DELETE_ASSIGNMENT_FILE =
    COURSE_CONTENT_BASE_ROUTE + '/assignment/deleteFileById/';
export const ADD_ASSIGNMENT_FILE = COURSE_CONTENT_BASE_ROUTE + '/assignment/addFile/';
export const GET_ASSIGNMENT_LINK = COURSE_CONTENT_BASE_ROUTE + '/assignment/getLinkById/';
export const DELETE_ASSIGNMENT_LINK =
    COURSE_CONTENT_BASE_ROUTE + '/assignment/deleteLinkById/';
export const ADD_ASSIGNMENT_LINK = COURSE_CONTENT_BASE_ROUTE + '/assignment/addLink/';

export const CREATE_MATERIAL = COURSE_CONTENT_BASE_ROUTE + '/material/create';
export const UPDATE_MATERIAL = COURSE_CONTENT_BASE_ROUTE + '/material/update/';
export const DELETE_MATERIAL = COURSE_CONTENT_BASE_ROUTE + '/material/delete/';
export const GET_BY_ID_MATERIAL = COURSE_CONTENT_BASE_ROUTE + '/material/getById/';
export const GET_ALL_MATERIAL = COURSE_CONTENT_BASE_ROUTE + '/material/getAll/';
export const GREMOVE_LIST_MATERIAL = COURSE_CONTENT_BASE_ROUTE + '/material/removeList';
export const GET_MATERIAL_FILE = COURSE_CONTENT_BASE_ROUTE + '/material/getFileById/';
export const DELETE_MATERIAL_FILE =
    COURSE_CONTENT_BASE_ROUTE + '/material/deleteFileById/';
export const ADD_MATERIAL_FILE = COURSE_CONTENT_BASE_ROUTE + '/material/addFile/';
export const GET_MATERIAL_LINK = COURSE_CONTENT_BASE_ROUTE + '/material/getFLinkById/';
export const DELETE_MATERIAL_LINK =
    COURSE_CONTENT_BASE_ROUTE + '/material/deleteLinkById/';
export const ADD_MATERIAL_LINK = COURSE_CONTENT_BASE_ROUTE + '/material/addLink/';

export const CREATE_TOPIC = COURSE_CONTENT_BASE_ROUTE + '/topic/create';
export const UPDATE_TOPIC = COURSE_CONTENT_BASE_ROUTE + '/topic/update/';
export const DELETE_TOPIC = COURSE_CONTENT_BASE_ROUTE + '/topic/delete/';
export const GET_BY_ID_TOPIC = COURSE_CONTENT_BASE_ROUTE + '/topic/getById/';
export const GET_ALL_TOPIC = COURSE_CONTENT_BASE_ROUTE + '/topic/getAll/';
