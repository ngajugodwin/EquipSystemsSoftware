import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CUSTOMER_URL, MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { Basket, IBasket, IBasketItem, IBasketTotals} from '../../../entities/models/basket';
import { AuthService } from '../auth-service/auth.service';
import { BehaviorSubject, map } from 'rxjs';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { IDeliveryMethod } from 'src/app/entities/models/deliveryMethod';
import {UpdateDeliveryMethod} from '../../../entities/models/updateDeliveryMethod';
import { ToasterService } from '../toaster-service/toaster.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {  
emptyBasket = {} as IBasket;
private basketSource = new BehaviorSubject<IBasket | null>(null);
private basketTotalSource = new BehaviorSubject<IBasketTotals | null>(null);
basket$ = this.basketSource.asObservable();
basketTotal$ = this.basketTotalSource.asObservable();
shipping = 0;

constructor(private http: HttpClient, private authService: AuthService, private toasterService: ToasterService) { }

createPaymentIntent(){
  return this.http.post<IBasket>(CUSTOMER_URL.BASE_URL + `/payments/${this.getCurrentBasket()?.id}`, {})
    .pipe(
      map((basket:IBasket) => {
        this.basketSource.next(basket);
      })
    );
}

setShippingPrice(deliveryMethod: IDeliveryMethod) {
  this.shipping = deliveryMethod.price;
  const basket = this.getCurrentBasket();
  if (basket) {
    basket.deliveryMethodId = deliveryMethod.id;
    basket.shippingPrice = deliveryMethod.price;
    this.calculateTotals();
    let updateDeliveryMethod = new UpdateDeliveryMethod(basket.id, basket.deliveryMethodId);
    this.updateDeliveryMethod(updateDeliveryMethod);
  }
}



updateDeliveryMethod(data: UpdateDeliveryMethod) {
 
  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket/setDeliveryMethod/${data.basketId}/${data.deliveryMethodId}`, {}).subscribe({
    next: (res) => {
      if (res) {
        this.updateBasketSource(res);
      }
    },
    error: (err: ErrorResponse) => {
      this.toasterService.showError(err.title, err.message);
    }
  })
    
}

deleteLocalBasket(id: number) {
  this.basketSource.next(null);
  this.basketTotalSource.next(null);
  localStorage.removeItem('basket_id');
}

getCurrentLoggedInUserBasket(userId: number) {
  return this.http.get<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket`)
  .subscribe({
    next: (basket: IBasket) => {
      if (basket) {
        this.updateBasketSource(basket);
      }
    },
    error: (err: ErrorResponse) => {
      this.toasterService.showError(err.title, err.message);
    }
  })
}

getBasket(basketId: number) {
  return this.http.get<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket/${basketId}`)
    .pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
        if (basket.shippingPrice) {
          this.shipping = basket.shippingPrice;
        }
       this.calculateTotals();
      })
    );
}

setBasket(basket: IBasket) {
  return this.http.post<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket`, basket)
      .subscribe({
        next: (basket: IBasket) => {
          this.updateBasketSource(basket);
        },
        error: (err: ErrorResponse) => {
          this.toasterService.showError(err.title, err.message);
        }
      })
}


getCurrentBasket() {
  return this.basketSource.value;
}

incrementOrDecrementItemQuantity(itemId: number, status: boolean) {

  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket/${this.getCurrentBasket()?.id}`, { itemId, status })
  .pipe(
    map((basket: IBasket) => {
      this.updateBasketSource(basket);
    })
  );
}

removeOneItemFromBasket(itemId: number) {  
  const userId = this.getCurrentBasket()?.id;
  const basketId = this.authService.getCurrentUser().id;
  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${basketId}/basket/${userId}/removeOneItemFromBasket/${itemId}`, {})
  .pipe(
    map((basket: IBasket) => {
    this.updateBasketSource(basket);
    })
  );
}

addItemToBasket(item: number, quantity = 1) {
  const itemToAdd: IBasketItem =this. mapItemtoBasketItem(item, quantity);
  

  let basket = this.getCurrentBasket();

  if (basket == null) {
    basket = this.createBasket();
  } 

 basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

  this.setBasket(basket);
}


private updateBasketSource(basket: IBasket){
  this.basketSource.next(basket);
  this.calculateTotals();
  if (!localStorage.getItem('basket_id')){
    localStorage.setItem('basket_id', basket.id.toString());
  }
}


private calculateTotals() {
  const basket = this.getCurrentBasket();
  const shipping = this.shipping;

 
  //  const subTotal = basket?.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
  let subTotal = basket?.items.reduce((a, b) => (b.price * b.quantity) + a, 0);

   const total = shipping + subTotal!;


  if (subTotal) {
    this.basketTotalSource.next({shipping, subTotal, total});
  }

}


private addOrUpdateItem (items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {

  const index = items.findIndex(i => i.itemId === itemToAdd.itemId);

  if (index === -1) {
   // itemToAdd.item = items[0].item; //to check code again
    itemToAdd.quantity = quantity;
    items.push(itemToAdd);
  } else {
    items[index].quantity += quantity;
  }

  return items;

}

private createBasket (): IBasket {
  // return new IBasket();
  const basket = new Basket();
  basket.userId = this.authService.getCurrentUser().id;
  // basket.id = Math.random();

  // localStorage.setItem('basket_id', basket.id.toString());

  return basket;
}

private mapItemtoBasketItem(itemId: number, quantity: number):IBasketItem {
  return  {
    itemId: itemId,
    quantity: quantity,
    price: 0,
    type: '',
    picture: '',
    name: ''
  }
}

deleteBasket(basketId: number) {
  return this.http.delete<boolean>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket/${basketId}`).subscribe(
    {
      next: (res) => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem('basket_id');
      }, 
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      } 
    }
  );
}


}


