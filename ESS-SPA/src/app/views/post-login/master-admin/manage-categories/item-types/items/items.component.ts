import { AfterViewInit, Component, OnInit, ViewContainerRef, OnDestroy, Input, OnChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QUERY } from 'src/app/constants/app.constant';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { IItemType } from 'src/app/entities/models/itemType';
import { Pagination } from 'src/app/entities/models/pagination';
import { ItemService } from 'src/app/shared/services/item-service/item.service';
import { ItemTypeService } from 'src/app/shared/services/itemTypes-service/item-type.service';
import { NgxSmartModalService } from 'ngx-smart-modal';
import { ItemComponent } from './item/item.component';
import { ConfirmComponent } from './confirm/confirm.component';
import { IModalDialogOptions, ModalDialogService } from 'ngx-modal-dialog';
import { Observable, Subject, Subscription } from 'rxjs';
import { NgxModalService } from "ngx-modalview";
import { DataUploadComponent } from '../../../data-upload/data-upload.component';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit, OnDestroy, OnChanges  {
  private $disposable = new Subscription;
  @Input() currentItemType: IItemType;

  items: IItem[] = [];
  itemParams: any = {};
  pagination: Pagination;
  
  constructor(private itemService: ItemService,
    private toasterService: ToasterService,
    private NgxModalService:NgxModalService) { }

  ngOnInit() {
    this.initItemParams();
  }

  onStatusChange(data: any) {
    this.getItems(data.target.value);
  }

  ngOnChanges() {
    this.getItems();
  }

  getItems(currentStatus?: number) {
   if (this.currentItemType && this.currentItemType !== null) {
    this.itemParams.status = (currentStatus === undefined) ? this.itemParams.status : currentStatus;
    this.itemService.getItems(this.currentItemType.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.itemParams).subscribe({
      next: (res) => {
        this.items = res.result;
        this.pagination = res.pagination;
      },
      error: (err: ErrorResponse) => {
       this.toasterService.showError(err.title, err.message);
      }
    })
   }
  }

  newItem() {
    if (this.currentItemType === undefined){
      alert('You must first select an item type');
      return;
    }
    this.$disposable = this.NgxModalService.addModal(ItemComponent, {
      title: 'New Item',
      message: '',
      data: {
        currentItemType: this.currentItemType,
      }
    })
    .subscribe((res: IItem)=>{
        if(res) {
            this.getItems(this.itemParams.status);
            this.toasterService.showSuccess('SUCCESS', 'New item created successfully');
        }
    });
  }

  onEditItem(item: IItem) {
    this.$disposable = this.NgxModalService.addModal(ItemComponent, {
      title: 'Edit Item',
      message: '',
      data: {
        item: item,
        currentItemType: this.currentItemType
      }
    })
    .subscribe((res: IItem)=>{
        if(res) {
            this.getItems(this.itemParams.status);
            this.toasterService.showInfo('SUCCESS', 'Item updated successfully');
        }
    });
  }

  onEnableDisableItem(itemId: number, status: boolean) {

    var res = (status) ? 'enable' : 'disable';
    let result = confirm(
      `Are you sure you want to ${res} this item?`
    );

    if (!result) {
      return;
    }

    this.itemService.activateOrDisableItem(this.currentItemType.id, itemId, status).subscribe({
      next: ((res) => {
        console.log(res);
         this.items.splice(this.items.findIndex(c => c.id === res.id), 1);
         if (res.status.toLocaleLowerCase() === 'active'){
          this.toasterService.showInfo('SUCCESS', 'Item enabled successfully');
         }
         this.toasterService.showInfo('SUCCESS', 'Item disabled successfully'); 
      }),
      error: ((error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      })
    });
  }

  onChangeItemPhoto(item: IItem) {
    this.$disposable = this.NgxModalService.addModal(DataUploadComponent, {
      title: 'Change Item Image',
      message: '',
      data: item
    })
    .subscribe((file: any)=>{
        if(file) {
            this.itemService.changeItemImage(this.currentItemType.id, item.id, file).subscribe({
              next: (res: IItem) => {
                if (res){
                  this.getItems(this.itemParams.status);
                  this.toasterService.showInfo('SUCCESS', 'Item image changed successfully');
                }
              },
              error: (err: ErrorResponse) => {
                this.toasterService.showError(err.title, err.message);
              }
            })           
        }
    });
  }

  ngOnDestroy(): void {
    this.$disposable.unsubscribe();
  }


  private initItemParams() {
    this.pagination = QUERY;
    this.itemParams.searchString = '';
    this.itemParams.status = 'Active';
  }

}

