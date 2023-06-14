import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ORG_ADMIN } from 'src/app/constants/api.constant';
import { PaginationResult } from 'src/app/entities/models/pagination';
import { IUser } from 'src/app/entities/models/user';

@Injectable({
  providedIn: 'root'
})
export class ManageAdminOrganisationService {

constructor(private http: HttpClient) { }

createOrganisationUserAccount(organisationId: number, userForCreation: IUser) {
  // let params = new HttpParams();
  // params = new HttpParams().set('isExternalReg', false)

  return this.http.post<IUser>(ORG_ADMIN.BASE_URL + `/${organisationId}`, userForCreation);
}

updateOrganisationUser(organisationId: number, userId: number, userToUpdate: IUser) {
   return this.http.put(ORG_ADMIN.BASE_URL + `/${organisationId}/${userId}`, userToUpdate);
}

getUser(organisationId: number, id: number): Observable<IUser> {
  return this.http.get<IUser>(ORG_ADMIN.BASE_URL + `/${organisationId}/${id}`);
}

activateOrDisableUser(organisationId: number, userId: number, newStatus: boolean): Observable<IUser> {
  let params = new HttpParams();
  params = new HttpParams().set('userStatus', newStatus);
  return this.http.put<IUser>(ORG_ADMIN.BASE_URL + `/${organisationId}/${userId}/setUserStatus`, {}, {params: params});
}

changeUserPassword(organisationId: number, userId: number, passwordData: object): Observable<IUser> {
  return this.http.put<IUser>(ORG_ADMIN.BASE_URL + `/${organisationId}/${userId}/changeUserPassword/`, passwordData);
}


getOrganisationUsers(organisationId: number, page?: number, itemsPerPage?: number, userParams?: any): Observable<PaginationResult<IUser[]>> {
  const paginatedResult: PaginationResult<IUser[]> = new PaginationResult<IUser[]>();
console.log(organisationId);
  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if (userParams !=  null) {
    params = params.append('searchString', userParams.searchString);

    if (userParams.organisationId > 0) {
      params = params.append('organisationId', userParams.organisationId);
    }

    if (userParams.status !=  null) {
      params = params.append('status', userParams.status);
    }
  }

  return this.http.get<IUser[]>(ORG_ADMIN.BASE_URL + `/${organisationId}`, {observe: 'response', params})
  .pipe(
    map((response: HttpResponse<IUser[]>) => {
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
