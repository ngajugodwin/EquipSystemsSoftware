import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {IItemType} from '../../../entities/models/itemType';
import { PaginationResult } from 'src/app/entities/models/pagination';
import { CUSTOMER_URL, MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs/internal/Observable';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ItemTypeService {
  
  private currentCategory = new BehaviorSubject<any>(null);

constructor(private http: HttpClient) { }

setCurrentItemType(itemType: IItemType) {
  this.currentCategory.next(itemType);
}

retrieveCurrentItemType(): Observable<IItemType> {
  return this.currentCategory.asObservable();
}

getItemType(categoryId: number, itemTypeId: number): Observable<IItemType> {
  return this.http.get<IItemType>(MASTER_ADMIN_URL.MANAGE_ITEM_TYPES + `/${categoryId}/ManageItemTypes/${itemTypeId}`);
}

createItemType(categoryId: number, itemTypeToCreate: IItemType) {
  return this.http.post<IItemType>(MASTER_ADMIN_URL.MANAGE_ITEM_TYPES + `/${categoryId}/ManageItemTypes`, itemTypeToCreate);
}

updateItemType(categoryId: number, itemTypeId: number, itemTypeToUpdate: IItemType) {
  return this.http.put<IItemType>(MASTER_ADMIN_URL.MANAGE_ITEM_TYPES + `/${categoryId}/ManageItemTypes/${itemTypeId}`, itemTypeToUpdate);
}

activateOrDisableItemType(categoryId: number, itemTypeId: number, newStatus: boolean): Observable<IItemType> {
  let params = new HttpParams();
  params = new HttpParams().set('itemTypeStatus', newStatus);
  return this.http.put<IItemType>(MASTER_ADMIN_URL.MANAGE_ITEM_TYPES + `/${categoryId}/ManageItemTypes/${itemTypeId}/changeItemTypeStatus`, {}, {params: params});
}

getItemTypes(categoryId: number, page?: number, itemsPerPage?: number, itemTypesParams?: any)  {
  console.log(categoryId);
  const paginatedResult: PaginationResult<IItemType[]> = new PaginationResult<IItemType[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if (itemTypesParams !=  null) {
    params = params.append('searchString', itemTypesParams.searchString);

    if (itemTypesParams.status !=  null) {
      params = params.append('status', itemTypesParams.status);
    }
  }

  return this.http.get<IItemType[]>(MASTER_ADMIN_URL.MANAGE_ITEM_TYPES + `/${categoryId}/ManageItemTypes`, {observe: 'response', params})
  .pipe(
    map((response: HttpResponse<IItemType[]>) => {
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

getItemTypeForCustomer(categoryId: number, itemTypeId: number) {
  return this.http.get<IItemType>(CUSTOMER_URL.BASE_URL + `/Items${itemTypeId}`);
}

getItemTypesForCustomer(categoryId: number, page?: number, itemsPerPage?: number, itemsParams?: any) {
  const paginatedResult: PaginationResult<IItemType[]> = new PaginationResult<IItemType[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if (itemsParams !=  null) {
    params = params.append('searchString', itemsParams.searchString);

    if (itemsParams.status !=  null) {
      params = params.append('status', itemsParams.status);
    }
  }

  return this.http.get<IItemType[]>(CUSTOMER_URL.BASE_URL + `/ItemTypes`, {observe: 'response', params})
  .pipe(
    map((response: HttpResponse<IItemType[]>) => {
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
