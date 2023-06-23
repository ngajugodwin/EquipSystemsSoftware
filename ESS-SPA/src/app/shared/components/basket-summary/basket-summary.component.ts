import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BasketService } from '../../services/basket-service/basket.service';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from 'src/app/entities/models/basket';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.css']
})
export class BasketSummaryComponent implements OnInit {
// @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
@Output() incrementDecrementValue: EventEmitter<any> = new EventEmitter<any>();
@Output() remove: EventEmitter<number> = new EventEmitter<number>();
@Input() isBasket = true;

   basket$: Observable<IBasket | null>;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
  }

  increaseOrDecreaseQuantity(itemId: number, status: boolean) {
    this.incrementDecrementValue.emit({itemId, status});
  }

  removeItemFromBasket(itemId: number) {
    this.remove.emit(itemId);
  }



}
