import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxModalComponent } from 'ngx-modalview';
import { ICategory } from 'src/app/entities/models/category';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItemType } from 'src/app/entities/models/itemType';
import { ModalData } from 'src/app/entities/models/modalData';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import { ItemTypeService } from 'src/app/shared/services/itemTypes-service/item-type.service';

@Component({
  selector: 'app-item-type',
  templateUrl: './item-type.component.html',
  styleUrls: ['./item-type.component.css']
})
export class ItemTypeComponent extends NgxModalComponent<ModalData, IItemType> implements OnInit, ModalData {
  title: string;
  message: string;
  data: any;

  currentCategory: ICategory;
  itemTypeForm: FormGroup;
  isSaving = false;

  constructor(private fb: FormBuilder,
    private itemTypeService: ItemTypeService) {
      super();
     }


  ngOnInit() {
    this.initItemTypeForm();
    this.checkForEditAction();   
  }

  initItemTypeForm() {
    this.itemTypeForm = this.fb.group({
      id: [null],
      name: ['', Validators.required]
    })
  }
  
  checkForEditAction() {
    if (this.data.itemType) {
      this.assignValuesToControl(this.data.itemType);
    }
  }

  assignValuesToControl(itemType: IItemType) {
    this.itemTypeForm.patchValue({
      id: itemType.id,
      name: itemType.name,
    });
  }

  save() {
    const itemType: IItemType = Object.assign({}, this.itemTypeForm.value);
    itemType.categoryId = this.data.currentCategory.id;
    if (this.itemTypeForm.valid) {
       if (this.itemTypeForm.value['id'] !== null) {
        this.itemTypeService.updateItemType(this.data.currentCategory.id, itemType.id, itemType).subscribe({
          next: (updatedItemType) => {
            this.isSaving = false;
            this.result = updatedItemType;
            console.log('Item type updated successfully' + updatedItemType) //TODO show success toaster message 
            this.close();
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       } else {      
        this.itemTypeService.createItemType(this.data.currentCategory.id, itemType).subscribe({
          next: (newItemType: IItemType) => {
            if (newItemType) {
              this.result = newItemType;
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
