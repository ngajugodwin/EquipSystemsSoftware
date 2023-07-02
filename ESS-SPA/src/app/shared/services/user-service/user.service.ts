import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { AUTH_URL, CUSTOMER_URL, USER_URL } from 'src/app/constants/api.constant';
import { PaginationResult } from 'src/app/entities/models/pagination';
import { IUser } from 'src/app/entities/models/user';
import { map } from 'rxjs';
import { UserParams } from 'src/app/entities/models/basequeryParams';
import { IAddress } from 'src/app/entities/models/address';

@Injectable({
  providedIn: 'root'
})
export class UserService {

constructor(private http: HttpClient) { }

createIndividualUserAccountV2(userForCreation: IUser, file: any) {

  const formData = new FormData();
  const headers = new HttpHeaders();
  
  headers.append('Content-Type', 'multipart/form-data');
  headers.append('Accept', 'application/json');

  if (file !== null || formData !== undefined) {
    formData.append('File', file, file.name);
    formData.append('userName', userForCreation.userName.toString());
    formData.append('email', userForCreation.email.toString());
    formData.append('firstName', userForCreation.firstName.toString());
    formData.append('lastName', userForCreation.lastName.toString());
    formData.append('city', userForCreation.city.toString());
    formData.append('state', userForCreation.state.toString());
    formData.append('accountType', userForCreation.accountType.toString());
    formData.append('street', userForCreation.street.toString());
    formData.append('password', userForCreation.password.toString());
    formData.append('isExternalReg', JSON.stringify(true));
    
  }
  const httpOptions = { headers: headers };

  return this.http.post<IUser>(AUTH_URL.REGISTER, formData, httpOptions);
}

createOrganisationUserAccountV2(userForCreation: IUser, file: any) {

  const formData = new FormData();
  const headers = new HttpHeaders();
  
  headers.append('Content-Type', 'multipart/form-data');
  headers.append('Accept', 'application/json');

  if (file !== null || formData !== undefined) {
    formData.append('File', file, file.name);
    formData.append('userName', userForCreation.userName.toString());
    formData.append('email', userForCreation.email.toString());
    formData.append('firstName', userForCreation.firstName.toString());
    formData.append('lastName', userForCreation.lastName.toString());
    formData.append('city', userForCreation.city.toString());
    formData.append('state', userForCreation.state.toString());
    formData.append('accountType', userForCreation.accountType.toString());
    formData.append('street', userForCreation.street.toString());
    formData.append('password', userForCreation.password.toString());
    formData.append('isExternalReg', JSON.stringify(true));

    if (userForCreation.organisation !== null) {
      formData.append('organisation.name', userForCreation.organisation.name.toString());
      formData.append('organisation.address', userForCreation.organisation.address.toString());
      formData.append('organisation.registrationNumber', userForCreation.organisation.registrationNumber.toString());      
      formData.append('organisation.dateOfIncorporation', userForCreation.organisation.dateOfIncorporation.toString());
    }
    
  }
  const httpOptions = { headers: headers };

  return this.http.post<IUser>(AUTH_URL.REGISTER, formData, httpOptions);
}

createUserAccount(userForCreation: IUser) {
  let params = new HttpParams();
  params = new HttpParams().set('isExternalReg', true)

  return this.http.post<IUser>(AUTH_URL.REGISTER, userForCreation,  {params: params});
}

// createUserAccount(userForCreation: IUser) {
//   let params = new HttpParams();
//   params = new HttpParams().set('isExternalReg', true)

//   return this.http.post<IUser>(AUTH_URL.REGISTER, userForCreation,  {params: params});
// }

approveUser(user: IUser) {
  return this.http.put<IUser>(USER_URL.SUPER_ADMIN_BASE_URL + `${user.id}/approveUser`, {});
}

rejectUser(user: IUser) {
  return this.http.put<IUser>(USER_URL.SUPER_ADMIN_BASE_URL + `${user.id}/rejectUser`, {});
}


onEnableDisableUser(userId: number, newStatus: boolean): Observable<IUser> {
  let params = new HttpParams();
  params = new HttpParams().set('userDeactivationStatus', newStatus);
  
  return this.http.put<IUser>(USER_URL.SUPER_ADMIN_BASE_URL + `${userId}/activateOrDisableUser`, {}, {params: params});
}


getUsers(page?: number, itemsPerPage?: number, userParams?: UserParams): Observable<PaginationResult<IUser[]>> {
  const paginatedResult: PaginationResult<IUser[]> = new PaginationResult<IUser[]>();

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

    if (userParams.accountType > 0) {
      params = params.append('accountType', userParams.accountType);
    }

    if (userParams.accountTypeName !=  null) {
      params = params.append('accountTypeName', userParams.accountTypeName);
    }

    if (userParams.status !=  null) {
      params = params.append('status', userParams.status);
    }
  }

  return this.http.get<IUser[]>(USER_URL.SUPER_ADMIN_BASE_URL, {observe: 'response', params})
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

getUser(id: number): Observable<IUser> {
  return this.http.get<IUser>(USER_URL.SUPER_ADMIN_BASE_URL + id);
}






}
