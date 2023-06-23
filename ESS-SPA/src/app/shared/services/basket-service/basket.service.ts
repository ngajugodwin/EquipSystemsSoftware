import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CUSTOMER_URL, MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { Basket, IBasket, IBasketItem, IBasketTotals} from '../../../entities/models/basket';
import { AuthService } from '../auth-service/auth.service';
import { BehaviorSubject, map } from 'rxjs';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { IDeliveryMethod } from 'src/app/entities/models/deliveryMethod';

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

constructor(private http: HttpClient, private authService: AuthService) { }


setShippingPrice(deliveryMethod: IDeliveryMethod) {
  this.shipping = deliveryMethod.price;
  this.calculateTotals();
}


deleteLocalBasket(id: number) {
  this.basketSource.next(null);
  this.basketTotalSource.next(null);
  localStorage.removeItem('basket_id');
}

getBasket(basketId: number) {
  return this.http.get<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket/${basketId}`)
    .pipe(
      map((basket: IBasket) => {
        this.basketSource.next(basket);
       this.calculateTotals();
      })
    );
}

// createItemType(categoryId: number, itemTypeToCreate: IItemType) {
//   return this.http.post<IItemType>(MASTER_ADMIN_URL.MANAGE_ITEM_TYPES + `/${categoryId}/ManageItemTypes`, itemTypeToCreate);
// }

setBasket(basket: IBasket) {



  return this.http.post<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket`, basket)
      .subscribe({
        next: (basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
          if (!localStorage.getItem('basket_id')){
            localStorage.setItem('basket_id', basket.id.toString());
          }
        },
        error: (err: ErrorResponse) => {
          console.log(err);
        }
      })
}

increateItemQuantity(isIncrease: boolean, itemId: number) {
  var data = {
    status: isIncrease,
    itemId: itemId
  }

  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket`, data)
      .subscribe({
        next: (basket: IBasket) => {
          this.basketSource.next(basket);
        },
        error: (err: ErrorResponse) => {
          console.log(err);
        }
      })
}

decreaseItemQuantity(isDecreaseIncrease: boolean, itemId: number) {
 var data = {
    status: isDecreaseIncrease,
    itemId: itemId
  }
  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket`, data)
      .subscribe({
        next: (basket: IBasket) => {
          this.basketSource.next(basket);
        },
        error: (err: ErrorResponse) => {
          console.log(err);
        }
      })
}

getCurrentBasket() {
  return this.basketSource.value;
}

incrementOrDecrementItemQuantity(itemId: number, status: boolean) {
  // public bool Status { get; set; }

  // public int ItemId { get; set; }

  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${this.authService.getCurrentUser().id}/basket/${this.getCurrentBasket()?.id}`, { itemId, status })
  .pipe(
    map((basket: IBasket) => {
      this.basketSource.next(basket);
     this.calculateTotals();
    })
  );
}

removeOneItemFromBasket(itemId: number) {  
  const userId = this.getCurrentBasket()?.id;
  const basketId = this.authService.getCurrentUser().id;
  return this.http.put<IBasket>(CUSTOMER_URL.BASE_URL + `/${basketId}/basket/${userId}/removeOneItemFromBasket/${itemId}`, {})
  .pipe(
    map((basket: IBasket) => {
      this.basketSource.next(basket);
     this.calculateTotals();
    })
  );
}

addItemToBasket(item: number, quantity = 1) {
  console.log(item);
  const itemToAdd: IBasketItem =this. mapItemtoBasketItem(item, quantity);
  

  let basket = this.getCurrentBasket();

  if (basket == null) {
    basket = this.createBasket();
  } 

  console.log

 basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

  this.setBasket(basket);
}


// addItemToBasket(item: number, quantity = 2) {
//   console.log(item);
//   const itemToAdd: IBasketItem =this. mapItemtoBasketItem(item, quantity);
  

//   let basket = this.getCurrentBasket();

//   if (basket == null) {
//     basket = this.createBasket();
//   }

//   console.log

//   basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);

//   this.setBasket(basket);
// }

private calculateTotals() {
  const basket = this.getCurrentBasket();
  const shipping = this.shipping;


 
  //  const subTotal = basket?.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
  const subTotal = Number(basket?.items.reduce((a, b) => (b.price * b.quantity) + a, 0));

   const total = shipping + Number(subTotal);


   this.basketTotalSource.next({shipping, subTotal, total});

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
        console.log(err);
      } 
    }
  );
}


}


