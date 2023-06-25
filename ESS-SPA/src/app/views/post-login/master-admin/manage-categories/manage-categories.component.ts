import { Component, OnDestroy, OnInit } from '@angular/core';
import {ICategory} from '../../../../entities/models/category';
import { Pagination } from 'src/app/entities/models/pagination';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { QUERY } from 'src/app/constants/app.constant';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { Router } from '@angular/router';
import { NgxModalService } from "ngx-modalview";
import { CategoryComponent } from './category/category.component';
import { Subscription } from 'rxjs';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';


@Component({
  selector: 'app-manage-categories',
  templateUrl: './manage-categories.component.html',
  styleUrls: ['./manage-categories.component.css']
})
export class ManageCategoriesComponent implements OnInit, OnDestroy {
  private $disposable = new Subscription;
  categoryParams: any = {};
  pagination: Pagination;  
  categories: ICategory[] = [];
  filterStatus: { id: number; name: string }[] = [];

  constructor(private categoryService: CategoryService, 
    private NgxModalService:NgxModalService,
    private toasterService: ToasterService,
    private router: Router) { }

  ngOnInit() {
    this.initFilter(EntityStatus);
    this.initCategoryParams();
    this.getCategories(this.categoryParams.status);
  }

  getCategories (currentStatus?: number) {
    this.categoryParams.status = currentStatus === undefined ? this.categoryParams.status : currentStatus;
    this.categoryService.getCategories(this.pagination.currentPage, this.pagination.itemsPerPage, this.categoryParams).subscribe({
      next: (res) => {
        if (res) {
          this.categories = res.result;
          this.pagination = res.pagination;
        }
      },
      error: (error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      }
    });
  }

  onStatusChange(data: any) {
    this.getCategories(data.target.value);
  }


  initCategoryParams() {    
   this.pagination = QUERY;
   this.categoryParams.searchString = '';
   this.categoryParams.status = EntityStatus.Active;
  }


  onNewCategory() {
   this.$disposable = this.NgxModalService.addModal(CategoryComponent, {
      title: 'New Category',
      message: '',
      data: {}
    })
    .subscribe((res: ICategory)=>{
        if(res) {
            this.getCategories(this.categoryParams.status);
            this.toasterService.showSuccess('SUCCESS', 'Category created successfully');
        }
    });
    
  }

  onEditCategory(category: ICategory) {
    this.$disposable = this.NgxModalService.addModal(CategoryComponent, {
      title: 'Edit Category',
      message: '',
      data: category
    })
    .subscribe((res: ICategory)=>{
        if(res) {
            this.getCategories(this.categoryParams.status);
            this.toasterService.showSuccess('SUCCESS', 'Category updated successfully');
        }
    });
  }

  onViewItemTypes(category: ICategory) {
    this.categoryService.setCurrentCategory(category);
    this.router.navigate([`/master-admin/manage-categories/${category.id}/item-types`]);
  }

  onEnableDisableCategory(categoryId: number, status: boolean) {

    var res = (status) ? 'enable' : 'disable';
    let result = confirm(
      `Are you sure you want to ${res} this category?`
    );

    if (!result) {
      return;
    }

    this.categoryService.activateOrDisableCategory(categoryId, status).subscribe({
      next: ((res) => {
         this.categories.splice(this.categories.findIndex(c => c.id === res.id), 1);
         if (res.status.toLocaleLowerCase() === 'active'){
          this.toasterService.showSuccess('SUCCESS', 'Category enabled successfully'); //TODO: show success toaster
         }
         this.toasterService.showInfo('SUCCESS', 'Category disabled successfully'); //TODO: show success toaster
      }),
      error: ((error: ErrorResponse) => {
       this.toasterService.showError(error.title, error.message);
      })
    });
  }

  initFilter(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.filterStatus.push({ id: <any>status[n], name: n });
        this.filterStatus = this.filterStatus.filter(x => x.name.toLowerCase() !== 'pending');
      }
    }
  }

  ngOnDestroy(): void {
    this.$disposable.unsubscribe();
  }

}
