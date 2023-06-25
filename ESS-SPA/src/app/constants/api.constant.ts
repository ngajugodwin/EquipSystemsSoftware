import {environment} from '../../environments/environment';

const SERVER_END_POINT = environment.SERVER_END_POINT;
const SUPER_ADMIN_SERVER_END_POINT = environment.SUPER_ADMIN_SERVER_END_POINT;
const ORG_ADMIN_SERVER_END_POINT = environment.ORG_ADMIN_SERVER_END_POINT;

export const AUTH_URL = {
    LOGIN: `${SERVER_END_POINT}/auth/login`,
    REGISTER: `${SERVER_END_POINT}/accounts/`,
    REFRESH_TOKEN: `${SERVER_END_POINT}/auth/refreshToken/`,
};

export const MASTER_ADMIN_URL = {
    MANAGE_ORGANISATION: `${SUPER_ADMIN_SERVER_END_POINT}/manageorganisations`,

    MANAGE_CATEGORIES: `${SUPER_ADMIN_SERVER_END_POINT}/managecategories`,
    MANAGE_ITEM_TYPES: `${SUPER_ADMIN_SERVER_END_POINT}`,
    MANAGE_ITEM: `${SUPER_ADMIN_SERVER_END_POINT}`,
    MANAGE_ORDERS: `${SUPER_ADMIN_SERVER_END_POINT}`
}

export const SELF_SERVICE_URL = {
    BASE_URL: `${SERVER_END_POINT}/users/`,
};

export const CUSTOMER_URL = {
    BASE_URL: `${SERVER_END_POINT}`
}

export const USER_URL = {
    SUPER_ADMIN_BASE_URL: `${SUPER_ADMIN_SERVER_END_POINT}/manageusers/`
}

export const ORG_ADMIN = {
    BASE_URL: `${ORG_ADMIN_SERVER_END_POINT}/manageadminorganisationusers`,
}

export const ROLE_URL = {
    BASE_URL: `${ORG_ADMIN_SERVER_END_POINT}/roles/`,
  };

// export const USER_URL2 = {
//     SUPER_ADMIN_BASE_URL: `${SUPER_ADMIN_SERVER_END_POINT}/manageusers/`
// }