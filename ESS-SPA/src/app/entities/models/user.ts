
import {IOrganisation} from './organisation';
import {AccountType} from './accountType';

export interface IUser {
    id: string,
    firstName: string,
    lastName: string,
    email: string,
    organisation: IOrganisation;
    accountType: AccountType
}