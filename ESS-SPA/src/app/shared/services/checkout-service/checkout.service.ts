import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {IDeliveryMethod} from '../../../entities/models/deliveryMethod';
import { Observable, map } from 'rxjs';
import { CUSTOMER_URL } from 'src/app/constants/api.constant';
import {IOrderToCreate} from '../../../entities/models/order';

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


}
