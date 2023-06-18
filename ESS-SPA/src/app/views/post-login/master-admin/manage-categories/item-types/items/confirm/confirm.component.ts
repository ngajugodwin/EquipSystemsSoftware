import { Component, ComponentRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IModalDialog, IModalDialogButton, IModalDialogOptions } from 'ngx-modal-dialog';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { IItemType } from 'src/app/entities/models/itemType';
import { ItemService } from 'src/app/shared/services/item-service/item.service';
import { ItemTypeService } from 'src/app/shared/services/itemTypes-service/item-type.service';
import { ItemTypesComponent } from '../../item-types.component';
import { ItemsComponent } from '../items.component';
import { NgxModalComponent } from 'ngx-modalview';
export interface ConfirmModel {
  title:string;
  message:string;
}
@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css']
})
export class ConfirmComponent extends NgxModalComponent<ConfirmModel, string> implements OnInit, ConfirmModel {
formTitle: string;
itemTypeForm: FormGroup;
isSaving = false;
title: string;
  message: string;

  constructor(private fb: FormBuilder, private itemTypeService: ItemTypeService, ) {
    super()
   }

   ngOnInit() {
    this.createForm();
   }

   confirm() {
    
    this.result = 'hello';
    this.close();
   }


  createForm() {
this.itemTypeForm = this.fb.group({
  id: [null],
  name: ['']
})
  }

  save() {

  }

  back(){}

}
