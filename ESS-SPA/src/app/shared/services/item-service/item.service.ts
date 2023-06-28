import { Injectable } from '@angular/core';
import {IItem} from '../../../entities/models/item';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { CUSTOMER_URL, MASTER_ADMIN_URL, USER_URL } from 'src/app/constants/api.constant';
import { PaginationResult } from 'src/app/entities/models/pagination';
import { ItemParams } from 'src/app/entities/models/itemParams';

@Injectable({
  providedIn: 'root'
})
export class ItemService {


  constructor(private http: HttpClient) { }


  changeItemImage(itemTypeId: number, itemId: number, file: any) {
    const formData = new FormData();
    const headers = new HttpHeaders();
    
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
  
    if (file !== null || formData !== undefined) {
      formData.append('File', file, file.name);
      formData.append('itemId', itemId.toString());      
    }

    const httpOptions = { headers: headers };

    return this.http.put<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}/changeItemImage`, formData, httpOptions);
  }

  getItem(itemTypeId: number, itemId: number): Observable<IItem> {
    return this.http.get<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}`);
  }
  
  createItem(itemTypeId: number, itemToCreate: IItem, file: any) {

    const formData = new FormData();
    const headers = new HttpHeaders();
    
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'application/json');
  
    if (file !== null || formData !== undefined) {
      formData.append('File', file, file.name);
      formData.append('Name', itemToCreate.name);
      formData.append('itemTypeId', itemToCreate.itemTypeId.toString());
      formData.append('categoryId', itemToCreate.categoryId.toString());
      formData.append('serialNumber', itemToCreate.serialNumber);
      formData.append('availableQuantity', itemToCreate.availableQuantity.toString());
      formData.append('price', itemToCreate.price.toString());
      
    }
    const httpOptions = { headers: headers };

    console.log(itemToCreate)
    return this.http.post<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems`, formData, httpOptions);
  }
  
  updateItem(itemTypeId: number, itemId: number, itemToUpdate: IItem) {
    return this.http.put<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}`, itemToUpdate);
  }
  
  activateOrDisableItem(itemTypeId: number, itemId: number, newStatus: boolean): Observable<IItem> {
    let params = new HttpParams();
    params = new HttpParams().set('itemStatus', newStatus);
    return this.http.put<IItem>(MASTER_ADMIN_URL.MANAGE_ITEM + `/${itemTypeId}/ManageItems/${itemId}/changeItemStatus`, {}, {params: params});
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

  getItemForCustomer(itemId: number) {
    return this.http.get<IItem>(CUSTOMER_URL.BASE_URL + `/Items/${itemId}`);
  }

  getItemsForCarouselDisplay() {
    return this.http.get<IItem[]>(CUSTOMER_URL.BASE_URL + `/Items/carouselDisplay`);
  }

  getItemsForCustomer(itemParams: ItemParams, page?: number, itemsPerPage?: number) {
    const paginatedResult: PaginationResult<IItem[]> = new PaginationResult<IItem[]>();
  
    let params = new HttpParams();
  
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }
  
    if (itemParams.itemTypeId !=  null) {
      params = params.append('itemTypeId', itemParams.itemTypeId.toString());
    }

    if (itemParams.categoryId !=  null) {
      params = params.append('categoryId', itemParams.categoryId.toString());
    }

    if (itemParams.search !=  null) {
      params = params.append('searchString', itemParams.search);
    }

    if (itemParams.sort) {
      params = params.append('sort', itemParams.sort);
    }
  
  
    return this.http.get<IItem[]>(CUSTOMER_URL.BASE_URL + `/Items`, {observe: 'response', params})
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
