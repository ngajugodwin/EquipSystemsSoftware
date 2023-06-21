import { Component, Input, OnInit } from '@angular/core';
import { IItem } from 'src/app/entities/models/item';
import {BasketService} from '../../../../shared/services/basket-service/basket.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent implements OnInit {
  @Input() item: IItem;

  constructor(private basketService: BasketService) { }

  ngOnInit() {
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.item.id);
  }

}
