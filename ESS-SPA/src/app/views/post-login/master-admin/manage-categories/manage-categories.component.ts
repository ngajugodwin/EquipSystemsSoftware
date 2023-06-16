import { Component, OnInit } from '@angular/core';
import {ICategory} from '../../../../entities/models/category';
import { Pagination } from 'src/app/entities/models/pagination';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { QUERY } from 'src/app/constants/app.constant';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-categories',
  templateUrl: './manage-categories.component.html',
  styleUrls: ['./manage-categories.component.css']
})
export class ManageCategoriesComponent implements OnInit {
  categoryParams: any = {};
  pagination: Pagination;  
  categories: ICategory[] = [];
  filterStatus: { id: number; name: string }[] = [];

  constructor(private categoryService: CategoryService, private router: Router) { }

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
        console.log(error);
      }
    });
  }

  onStatusChange(data: any) {
    console.log(data.target.value);
    this.getCategories(data.target.value);
  }


  initCategoryParams() {    
   this.pagination = QUERY;
   this.categoryParams.searchString = '';
   this.categoryParams.status = EntityStatus.Active;
  }

  onEditCategory(categoryId: number) {
    this.router.navigate([`/master-admin/manage-categories/edit/${categoryId}`])
  }

  onViewItemTypes(categoryId: number) {}

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
          console.log('Category enabled successfully'); //TODO: show success toaster
         }
         console.log('Category disabled successfully'); //TODO: show success toaster
      }),
      error: ((error: ErrorResponse) => {
        console.log(error); //TODO: show error toaster
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


}
