import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxModalComponent } from 'ngx-modalview';
import { ICategory } from 'src/app/entities/models/category';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import {ModalData} from '../../../../../entities/models/modalData';

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

  constructor(private fb: FormBuilder, private router: Router, 
    private categoryService: CategoryService,
    private activatedRoute: ActivatedRoute) { 
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
    })
  }

  checkForEditAction() {
    if (this.data) {
      this.assignValuesToControl(this.data);
      this.formTitle = 'Edit Item Type';
    }
  }

  save() {

    const category: ICategory = Object.assign({}, this.categoryForm.value);

    if (this.categoryForm.valid) {
       if (this.categoryForm.value['id'] !== null) {
        this.categoryService.updateCategory(category.id, category).subscribe({
          next: (updatedCategory) => {
            this.isSaving = false;
            this.result = updatedCategory;
            this.closeDialog(); 
            console.log('Category updated successfully' + updatedCategory) //TODO show success toaster message 
            
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       } else {      
        this.categoryService.createCategory(category).subscribe({
          next: (newCategory: ICategory) => {
            if (newCategory) {
              this.result = newCategory;
              console.log(newCategory) //TODO show success toaster     
              this.closeDialog();  
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
    // this.router.navigate(['/master-admin/manage-categories']);
  }

  assignValuesToControl(category: ICategory) {
    this.categoryForm.patchValue({
      id: category.id,
      name: category.name,
    });
  }

}
