import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {IDeliveryMethod} from '../../../entities/models/deliveryMethod';
import { Observable, map } from 'rxjs';
import { CUSTOMER_URL } from 'src/app/constants/api.constant';
import {IOrder, IOrderToCreate} from '../../../entities/models/order';
import { PaginationResult } from 'src/app/entities/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

constructor(private http: HttpClient) { }

createOrder(order: IOrderToCreate) {
  return this.http.post<IOrderToCreate>(CUSTOMER_URL.BASE_URL + `/orders/`, order);
}

getDeliveryMethods() {
  return this.http.get<IDeliveryMethod[]>(CUSTOMER_URL.BASE_URL + `/orders/getDeliveryMethods`).pipe(  
    map((dm: IDeliveryMethod[]) => {
      return dm.sort((a, b) => b.price - a.price);
    })
  )
}

getOrderDetails(orderId: number) {
  return this.http.get<IOrder>(CUSTOMER_URL.BASE_URL + `/orders/${orderId}`);
}

getOrdersForUser(page?: number, itemsPerPage?: number, ordersParams?: any)  {
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
  }

  return this.http.get<IOrder[]>(CUSTOMER_URL.BASE_URL + `/orders`, {observe: 'response', params})
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
