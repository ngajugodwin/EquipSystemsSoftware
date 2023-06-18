import { Injectable } from '@angular/core';
import {IItem} from '../../../entities/models/item';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { PaginationResult } from 'src/app/entities/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ItemService {


  constructor(private http: HttpClient) { }


  getItem(itemTypeId: number, itemId: number): Observable<IItem> {
    return this.http.get<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}`);
  }
  
  createItem(itemTypeId: number, itemToCreate: IItem) {
    return this.http.post<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems`, itemToCreate);
  }
  
  updateItem(itemTypeId: number, itemId: number, itemToUpdate: IItem) {
    return this.http.put<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}`, itemToUpdate);
  }
  
  activateOrDisableItem(itemTypeId: number, itemId: number, newStatus: boolean): Observable<IItem> {
    let params = new HttpParams();
    params = new HttpParams().set('itemStatus', newStatus);
    return this.http.put<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}/setItemStatus`, {}, {params: params});
  }
  
  getItems(itemTypeId: number, page?: number, itemsPerPage?: number, itemsParams?: any)  {
    const paginatedResult: PaginationResult<IItem[]> = new PaginationResult<IItem[]>();
  
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
  
    return this.http.get<IItem[]>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems`, {observe: 'response', params})
    .pipe(
      map((response: HttpResponse<IItem[]>) => {
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
