import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxModalComponent } from 'ngx-modalview';
import { ICategory } from 'src/app/entities/models/category';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import {ModalData} from '../../../../../entities/models/modalData';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent extends NgxModalComponent<ModalData, ICategory> implements OnInit, ModalData  {
  title: string;
  message: string;
  data: ICategory;

  formTitle: string;
  categoryForm: FormGroup;
  isSaving = false;

  constructor(private fb: FormBuilder,
    private categoryService: CategoryService,
    private toasterService: ToasterService) { 
      super();
    }

  ngOnInit() {
    this.initCategoryForm();
    this.checkForEditAction();
  }

  initCategoryForm() {
    this.categoryForm = this.fb.group({
      id: [null],
      name: ['', Validators.required]
    });
  }

  checkForEditAction() {
    if (this.data) {
      this.assignValuesToControl(this.data);
    }
  }

  save() {

    const category: ICategory = Object.assign({}, this.categoryForm.value);

    if (this.categoryForm.valid) {
       if (this.categoryForm.value['id'] !== undefined) {
        this.categoryService.updateCategory(category.id, category).subscribe({
          next: (updatedCategory) => {
            this.isSaving = false;
            this.result = updatedCategory;
            this.closeDialog(); 
            this.toasterService.showInfo('SUCCESS', 'Category updated successfully');            
          },
          error: (error: ErrorResponse) => {
            this.toasterService.showError(error.title, error.message);
          }
        })
       } else {      
        console.log(category);
        this.categoryService.createCategory(category).subscribe({
          next: (newCategory: ICategory) => {
            if (newCategory) {
              this.result = newCategory;
              this.toasterService.showInfo('SUCCESS', 'Category created successfully');
              this.closeDialog();  
            }
          },
          error: (error: ErrorResponse) => {
            this.toasterService.showError(error.title, error.message);
          }
        })

       }
     }
   }

  closeDialog() {
    this.close();
  }

  assignValuesToControl(category: ICategory) {
    this.categoryForm.patchValue({
      id: category.id,
      name: category.name,
    });
  }

}
