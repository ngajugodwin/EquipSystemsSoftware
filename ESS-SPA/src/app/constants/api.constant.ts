import {environment} from '../../environments/environment';

const SERVER_END_POINT = environment.SERVER_END_POINT;
const SUPER_ADMIN_SERVER_END_POINT = environment.SUPER_ADMIN_SERVER_END_POINT;
const ORG_ADMIN_SERVER_END_POINT = environment.ORG_ADMIN_SERVER_END_POINT;

export const AUTH_URL = {
    LOGIN: `${SERVER_END_POINT}/auth/login`,
    REGISTER: `${SERVER_END_POINT}/accounts/`,
    REFRESH_TOKEN: `${SERVER_END_POINT}/auth/refreshToken/`,
};

export const ORG_URL = {
    BASE_URL: `${SUPER_ADMIN_SERVER_END_POINT}/manageorganisations/`
}

export const USER_URL = {
    SUPER_ADMIN_BASE_URL: `${SUPER_ADMIN_SERVER_END_POINT}/manageusers/`

   
}

export const ORG_ADMIN = {
    BASE_URL: `${ORG_ADMIN_SERVER_END_POINT}/manageorganisationusers`
}

// export const USER_URL2 = {
//     SUPER_ADMIN_BASE_URL: `${SUPER_ADMIN_SERVER_END_POINT}/manageusers/`
// }