import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasket } from 'src/app/entities/models/basket';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {
  basket$: Observable<IBasket | null>;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
  }

  increaseOrDecreaseQuantity(result: any) {
    this.basketService.incrementOrDecrementItemQuantity(result.itemId, result.status).subscribe({
      next: (res) => {
       console.log(res);
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }

  removeItemFromBasket(itemId: number) {    
    let result = confirm(
      `Are you sure you want to remove this item from your basket??`
    );

    if (!result) {
      return;
    }

    this.basketService.removeOneItemFromBasket(itemId).subscribe({
      next: (res) => {
       console.log(res);
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }

}
