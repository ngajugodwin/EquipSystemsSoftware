import {EntityStatus} from './entityStatus';

export interface BaseQueryParams {
   
}

export interface OrganisationParams extends BaseQueryParams {
   status: EntityStatus,
   searchString: string
}