import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { ItemService } from 'src/app/shared/services/item-service/item.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  item: IItem;

  constructor(private itemService: ItemService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.getItem();
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
