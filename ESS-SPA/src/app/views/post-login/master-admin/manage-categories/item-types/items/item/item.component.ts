import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgxModalComponent } from 'ngx-modalview';
import { ICategory } from 'src/app/entities/models/category';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { ModalData } from 'src/app/entities/models/modalData';
import { ItemService } from 'src/app/shared/services/item-service/item.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent extends NgxModalComponent<ModalData, IItem> implements OnInit, ModalData  {
  title: string;
  message: string;
  data: any;
  itemForm: FormGroup;
  isSaving = false;

  constructor(private fb: FormBuilder, private itemService: ItemService) { 
    super();
  }


  ngOnInit() {
    this.initItemForm();
    this.checkForEditAction();
  }

  checkForEditAction() {
    if (this.data.item) {
      this.assignValuesToControl(this.data.item);
    }
  }

  assignValuesToControl(item: IItem) {
    this.itemForm.patchValue({
      id: item.id,
      name: item.name,
      serialNumber: item.serialNumber
    });
  }


  initItemForm() {
    this.itemForm = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      serialNumber: ['', Validators.required]
    })
  }

  save() {
    const item: IItem = Object.assign({}, this.itemForm.value);
    item.itemTypeId = this.data.currentItemType.id;
    if (this.itemForm.valid) {
       if (this.itemForm.value['id'] !== null) {
        this.itemService.updateItem(this.data.currentItemType.id, item.id, item).subscribe({
          next: (updatedItem) => {
            this.isSaving = false;
            this.result = updatedItem;
            console.log('Item type updated successfully' + updatedItem) //TODO show success toaster message 
            this.close();
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       } else {      
        this.itemService.createItem(this.data.currentItemType.id, item).subscribe({
          next: (newItem: IItem) => {
            if (newItem) {
              this.result = newItem;
              this.close();
            }
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       }
     }
  }

  closeDialog() {
    this.close();
  }
}
