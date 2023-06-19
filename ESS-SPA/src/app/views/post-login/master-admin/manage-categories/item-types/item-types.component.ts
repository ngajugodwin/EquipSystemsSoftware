import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
// import { ActivatedRoute, Router } from '@angular/router';
import { QUERY } from 'src/app/constants/app.constant';
import { ICategory } from 'src/app/entities/models/category';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItemType } from 'src/app/entities/models/itemType';
import { Pagination } from 'src/app/entities/models/pagination';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import { ItemTypeService } from 'src/app/shared/services/itemTypes-service/item-type.service';
import { NgxModalService } from "ngx-modalview";
import { ItemTypeComponent } from './item-type/item-type.component';

@Component({
  selector: 'app-item-types',
  templateUrl: './item-types.component.html',
  styleUrls: ['./item-types.component.css']
})
export class ItemTypesComponent implements OnInit, OnDestroy {
  private $disposable = new Subscription;
  selectedItemType: IItemType;
  currentCategory: ICategory;
  itemTypes: IItemType[] = [];
  itemTypeParams: any = {};
  pagination: Pagination;
  filterStatus: { id: number; name: string }[] = [];

  constructor(private activatedRoute: ActivatedRoute, private router: Router, 
    private NgxModalService:NgxModalService,
      private itemTypesService: ItemTypeService) { }

  ngOnInit() {   
    this.initItemTypeParams();
    this.getCategory();
  }
  
  getCategory() {
    this.activatedRoute.data.subscribe(data => {
    var category: ICategory = data['category'];
    if (category) {
      this.currentCategory = category;
      this.getItemTypes();
    }
   });
  }

  onStatusChange(data: any) {
    this.getItemTypes(data.target.value);

  }

  getItemTypes(currentStatus?: number) {
    this.itemTypeParams.status = (currentStatus === undefined) ? this.itemTypeParams.status : currentStatus;
    this.itemTypesService.getItemTypes(this.currentCategory.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.itemTypeParams).subscribe({
      next: (res) => {
        this.itemTypes = res.result;
        this.pagination = res.pagination;
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }

  newItemType() {
    this.$disposable = this.NgxModalService.addModal(ItemTypeComponent, {
      title: 'New Item Type',
      message: '',
      data: {
        currentCategory: this.currentCategory
      }
    })
    .subscribe((res: IItemType)=>{
        if(res) {
            this.getItemTypes(this.itemTypeParams.status);
        }
    });
  }

  onEditItemType(itemType: IItemType) {
    this.$disposable = this.NgxModalService.addModal(ItemTypeComponent, {
      title: 'Edit Item Type',
      message: '',
      data: {
        itemType: itemType,
        currentCategory: this.currentCategory
      }
    })
    .subscribe((res: IItemType)=>{
        if(res) {
            this.getItemTypes(this.itemTypeParams.status);
        }
    });
  }

  onEnableDisableItemType(itemTypeId: number, status: boolean) {

    var res = (status) ? 'enable' : 'disable';
    let result = confirm(
      `Are you sure you want to ${res} this item type?`
    );

    if (!result) {
      return;
    }

    this.itemTypesService.activateOrDisableItemType(this.currentCategory.id, itemTypeId, status).subscribe({
      next: ((res) => {
        console.log(res);
         this.itemTypes.splice(this.itemTypes.findIndex(c => c.id === res.id), 1);
         if (res.status.toLocaleLowerCase() === 'active'){
          console.log('Item type enabled successfully'); //TODO: show success toaster
         }
         console.log('Item type disabled successfully'); //TODO: show success toaster
      }),
      error: ((error: ErrorResponse) => {
        console.log(error); //TODO: show error toaster
      })
    });
  }

 
  onSelectedItemType(itemType: IItemType) {
    
    this.itemTypesService.getItemType(this.currentCategory.id, itemType.id).subscribe((res: IItemType) => {
      this.setItemTypeId(res);
    })
  }


  
  ngOnDestroy(): void {
    this.$disposable.unsubscribe();
  }

  private setItemTypeId(itemType: IItemType) {
    this.selectedItemType = itemType;
  }


  private initItemTypeParams() {
    this.pagination = QUERY;
    this.itemTypeParams.searchString = '';
    this.itemTypeParams.status = 'Active';
  }

}
