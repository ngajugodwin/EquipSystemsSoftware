import {environment} from '../../environments/environment';

const SERVER_END_POINT = environment.SERVER_END_POINT;
const SUPER_ADMIN_SERVER_END_POINT = environment.SUPER_ADMIN_SERVER_END_POINT;

export const AUTH_URL = {
    LOGIN: `${SERVER_END_POINT}/auth/login/`,
    REGISTER: `${SERVER_END_POINT}/auth/register/`,
    REFRESH_TOKEN: `${SERVER_END_POINT}/auth/refreshToken/`,
};