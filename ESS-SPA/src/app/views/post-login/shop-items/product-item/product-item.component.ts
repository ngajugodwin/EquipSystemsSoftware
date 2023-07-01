import { Component, Input, OnInit } from '@angular/core';
import { IItem } from 'src/app/entities/models/item';
import {BasketService} from '../../../../shared/services/basket-service/basket.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent implements OnInit {
  @Input() item: IItem;

  constructor(private basketService: BasketService, private toasterService: ToasterService) { }

  ngOnInit() {
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.item.id);
    this.toasterService.showInfo('SUCCESS', `${this.item.name} has been added to your basket`);
  }

}
