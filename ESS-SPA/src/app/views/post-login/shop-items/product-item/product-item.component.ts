import { Component, Input, OnInit } from '@angular/core';
import { IItem } from 'src/app/entities/models/item';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent implements OnInit {
  @Input() item: IItem;

  constructor() { }

  ngOnInit() {
  }

}
