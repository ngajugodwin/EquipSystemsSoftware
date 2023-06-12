import { AccountType } from './accountType';
import {EntityStatus} from './entityStatus';

export interface BaseQueryParams {
   
}

export interface OrganisationParams extends BaseQueryParams {
   status: EntityStatus,
   searchString: string
}

export interface UserParams extends BaseQueryParams {
   status: EntityStatus,
   searchString: string,
   organisationId: number,
   accountType: AccountType,
   accountTypeName: string,
}