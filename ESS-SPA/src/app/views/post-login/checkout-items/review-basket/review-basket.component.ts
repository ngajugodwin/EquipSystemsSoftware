import { Component, OnInit } from '@angular/core';
import { NgxModalComponent } from 'ngx-modalview';
import { IBasket } from 'src/app/entities/models/basket';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { ModalData } from 'src/app/entities/models/modalData';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';

@Component({
  selector: 'app-review-basket',
  templateUrl: './review-basket.component.html',
  styleUrls: ['./review-basket.component.css']
})
export class ReviewBasketComponent extends NgxModalComponent<ModalData, IBasket> implements  OnInit, ModalData {
  title: string;
  message: string;
  data: any;

  constructor(private basketService: BasketService) {
    super();
   }
 

  ngOnInit() {
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

  closeDialog() {
    this.close();
  }

}
