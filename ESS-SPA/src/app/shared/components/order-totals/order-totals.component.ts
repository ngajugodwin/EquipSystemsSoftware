import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IBasketTotals } from 'src/app/entities/models/basket';
import { BasketService } from '../../services/basket-service/basket.service';

@Component({
  selector: 'app-order-totals',
  templateUrl: './order-totals.component.html',
  styleUrls: ['./order-totals.component.css']
})
export class OrderTotalsComponent implements OnInit {
  basketTotal$: Observable<IBasketTotals | null>;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
    this.basketTotal$ = this.basketService.basketTotal$;
  }

}
