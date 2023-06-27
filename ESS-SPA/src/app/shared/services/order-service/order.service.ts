import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { IOrder } from 'src/app/entities/models/order';
import { PaginationResult } from 'src/app/entities/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

constructor(private http: HttpClient) { }

approveOrder(orderId: number) {
  return this.http.post<IOrder>(MASTER_ADMIN_URL.MANAGE_ORDERS + `/manageorders/${orderId}`, {});
}

closeOrder(orderId: number) {
  return this.http.post<IOrder>(MASTER_ADMIN_URL.MANAGE_ORDERS + `/manageorders/closeOrder/${orderId}`, {});
}

rejectOrder(orderId: number) {
  return this.http.delete<IOrder>(MASTER_ADMIN_URL.MANAGE_ORDERS + `/manageorders/${orderId}`);
}


getOrdersForModeration(page?: number, itemsPerPage?: number, ordersParams?: any): Observable<PaginationResult<IOrder[]>>  {
  const paginatedResult: PaginationResult<IOrder[]> = new PaginationResult<IOrder[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if (ordersParams !=  null) {
    params = params.append('searchString', ordersParams.searchString);

    if (ordersParams.status !=  null) {
      params = params.append('status', ordersParams.status);
    }

    if (ordersParams.bookingStatus !=  null) {
      params = params.append('bookingStatus', ordersParams.bookingStatus);
    }
  }

  return this.http.get<IOrder[]>(MASTER_ADMIN_URL.MANAGE_ORDERS + `/manageOrders`, {observe: 'response', params})
  .pipe(
    map((response: HttpResponse<IOrder[]>) => {
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
