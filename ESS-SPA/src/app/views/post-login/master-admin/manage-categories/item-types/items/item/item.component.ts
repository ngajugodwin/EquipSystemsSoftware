import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { NgxModalComponent } from 'ngx-modalview';
import { MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { ModalData } from 'src/app/entities/models/modalData';
import { ItemService } from 'src/app/shared/services/item-service/item.service';
// import { FileUploader } from 'ng2-file-upload';

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
  currentPhoto: string;
  isEditMode = false;
  selectedFile: any;


  constructor(private fb: FormBuilder, private itemService: ItemService) { 
    super();

  }

  ngOnInit() {
    this.initItemForm();
    this.checkForEditAction();
  }

  checkForEditAction() {
    if (this.data.item) {
      console.log(this.data.item);
      this.assignValuesToControl(this.data.item);
    }
  }


  assignValuesToControl(item: IItem) {
    this.isEditMode = true;
    this.itemForm.patchValue({
      id: item.id,
      name: item.name,
      price: item.price,
      availableQuantity: item.availableQuantity,
      serialNumber: item.serialNumber,
      file: ''      
    });
    this.itemForm.get('file')?.setValidators([]);
    this.currentPhoto = item.url;
  }


  initItemForm() {
    this.itemForm = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      serialNumber: ['', Validators.required],
      price: ['', Validators.required],
      availableQuantity: ['', Validators.required],
      url: [''],
      file: ['', Validators.required]
    })
  }

  OnFileSelected(data: any) {
console.log(data.target.value);
  }


  onFileChanged(event: any) {
    this.selectedFile = event.target.files[0]
  }

  save() {

    if (this.isEditMode) {

    } else {
      if (this.selectedFile === null || this.selectedFile === undefined) {
        alert('Please select a file');
        return;
      }  
      
      if (!this.selectedFile.name.includes('.jpg', 0)) {
        alert('Only jpg pictures are supported');
        // TODO: display notification when file is not supported or in right format
        return;
      }
    }

   
    const item: IItem = Object.assign({}, this.itemForm.value);
    item.itemTypeId = this.data.currentItemType.id;
    item.categoryId = this.data.currentItemType.categoryId;

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
        console.log(item);
        this.itemService.createItem(this.data.currentItemType.id, item, this.selectedFile).subscribe({
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
