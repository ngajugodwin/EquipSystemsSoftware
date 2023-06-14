
import {IOrganisation} from './organisation';
import {AccountType} from './accountType';
import {IRole} from './role';

export interface IUser {
    id: number,
    firstName: string,
    lastName: string,
    userName: string,
    email: string,
    organisationId: number;
    organisation: IOrganisation;
    accountType: AccountType,
    status: string,
    typeName: string,
    password: string,
    userRoles: IRole[];
}