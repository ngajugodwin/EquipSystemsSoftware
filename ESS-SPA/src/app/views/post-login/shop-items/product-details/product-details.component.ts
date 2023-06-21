import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { BasketService } from 'src/app/shared/services/basket-service/basket.service';
import { ItemService } from 'src/app/shared/services/item-service/item.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  quantity = 1;
  item: IItem;


  constructor(private itemService: ItemService, private basketService: BasketService,
     private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.getItem();
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.item.id, this.quantity);
  }

  increaseOrDecreaseQuantity(status: boolean) {
    if (status) {
      this.quantity++;
    } else{
     if (this.quantity > 1) {
      this.quantity--;
     }
    }
  }

  getItem() {
    this.itemService.getItemForCustomer(Number(this.activatedRoute.snapshot.paramMap.get('id'))).subscribe({
      next: (res) => {
        if (res) {
         this.item = res;
        }
      }, error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
      }

}
