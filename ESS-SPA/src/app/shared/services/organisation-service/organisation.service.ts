import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
// import { ORG_URL } from 'src/app/constants/api.constant';
import { IOrganisation } from 'src/app/entities/models/organisation';
import {OrganisationParams} from '../../../entities/models/basequeryParams';
import {PaginationResult} from '../../../entities/models/pagination';
import { get } from 'http';
import { IUser } from 'src/app/entities/models/user';
import { map } from 'rxjs';
import { MASTER_ADMIN_URL } from 'src/app/constants/api.constant';

@Injectable({
  providedIn: 'root'
})
export class OrganisationService {

constructor(private http: HttpClient) { }


getOrganisation(organisationId: number): Observable<IOrganisation> {
  return this.http.get<IOrganisation>(MASTER_ADMIN_URL.MANAGE_ORGANISATION + `${organisationId}`);
}

updateOrganisation(organisationId: number, organisationToUpdate: IOrganisation) {
  return this.http.put<IOrganisation>(MASTER_ADMIN_URL.MANAGE_ORGANISATION + `${organisationId}`, organisationToUpdate);
}

activateOrDisableOrganisation(organisationId: number, newStatus: boolean): Observable<IOrganisation> {
  let params = new HttpParams();
  params = new HttpParams().set('organisationStatus', newStatus);
  console.log(params);
  return this.http.put<IOrganisation>(MASTER_ADMIN_URL.MANAGE_ORGANISATION + `/${organisationId}/setOrganisationStatus`, {}, {params: params});
}

rejectOrganisation(organisationId: number) {
  return this.http.delete<IOrganisation>(MASTER_ADMIN_URL.MANAGE_ORGANISATION + `${organisationId}`);
}

getOrganisations(page?: number, itemsPerPage?: number, organisationParams?: OrganisationParams)  {
  const paginatedResult: PaginationResult<IOrganisation[]> = new PaginationResult<IOrganisation[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if (organisationParams !=  null) {
    params = params.append('searchString', organisationParams.searchString);

    if (organisationParams.status !=  null) {
      params = params.append('status', organisationParams.status);
    }
  }

  return this.http.get<IOrganisation[]>(MASTER_ADMIN_URL.MANAGE_ORGANISATION, {observe: 'response', params})
  .pipe(
    map((response: HttpResponse<IOrganisation[]>) => {
      if (response.body) {
        paginatedResult.result = response.body;
      }
      if (response.headers.get('Pagination') != null) {
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') || '');
      }
      return paginatedResult;
    })
  );
}



}
