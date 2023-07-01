import { Component, OnInit } from '@angular/core';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { ItemService } from 'src/app/shared/services/item-service/item.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-items-display',
  templateUrl: './items-display.component.html',
  styleUrls: ['./items-display.component.css']
})
export class ItemsDisplayComponent implements OnInit {
  items: IItem[] = [];

  constructor(private itemService: ItemService, 
    private toasterService: ToasterService) { }

  ngOnInit() {
    this.getItems();
  }

  getItems() {
    this.itemService.getItemsForCarouselDisplay().subscribe({
      next: (res) => {
        if (res){
          this.items = res;
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

}
