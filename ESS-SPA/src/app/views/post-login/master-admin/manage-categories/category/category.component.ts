import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ICategory } from 'src/app/entities/models/category';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  formTitle: string;
  categoryForm: FormGroup;
  isSaving = false;

  constructor(private fb: FormBuilder, private router: Router, 
    private categoryService: CategoryService,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.initCategoryForm();
    this.getCategory();
  }

  initCategoryForm() {
    this.categoryForm = this.fb.group({
      id: [null],
      name: ['', Validators.required]
    },  {validator: [this.passwordMatchValidator]} as AbstractControlOptions)
  }

  private passwordMatchValidator(f: FormGroup) {
    return f.get('password')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }

  save() {

    const category: ICategory = Object.assign({}, this.categoryForm.value);

    if (this.categoryForm.valid) {
       if (this.categoryForm.value['id'] !== null) {
        this.categoryService.updateCategory(category.id, category).subscribe({
          next: (updatedCategory) => {
            this.isSaving = false;
            console.log('Category updated successfully' + updatedCategory) //TODO show success toaster message 
            this.router.navigate(['/master-admin/manage-categories']);
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       } else {      
        this.categoryService.createCategory(category).subscribe({
          next: (newCategory: ICategory) => {
            if (newCategory) {
              console.log(newCategory) //TODO show success toaster
              this.router.navigate(['/master-admin/manage-categories']);
            }
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       }
     }
   }

  cancel() {
    this.router.navigate(['/master-admin/manage-categories']);
  }

  getCategory() {
    this.activatedRoute.data.subscribe(data => {
    const category = data['category'];
    if (category) {
      this.formTitle = 'Edit Category';
      console.log(category);
      this.assignValuesToControl(category);
    } else {
      this.formTitle = 'New Category';
    }
   });
  }

  assignValuesToControl(category: ICategory) {
    this.categoryForm.patchValue({
      id: category.id,
      name: category.name,
    });
  }

}
