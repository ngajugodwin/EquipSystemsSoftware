import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { QUERY } from 'src/app/constants/app.constant';
import { ICategory } from 'src/app/entities/models/category';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IItem } from 'src/app/entities/models/item';
import { IItemType } from 'src/app/entities/models/itemType';
import { Pagination } from 'src/app/entities/models/pagination';
import { CategoryService } from 'src/app/shared/services/category-service/category.service';
import { ItemService } from 'src/app/shared/services/item-service/item.service';
import { ItemTypeService } from 'src/app/shared/services/itemTypes-service/item-type.service';
import {CategoryParams, ItemParams, ItemTypeParams} from '../../../entities/models/itemParams';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-shop-items',
  templateUrl: './shop-items.component.html',
  styleUrls: ['./shop-items.component.css']
})
export class ShopItemsComponent implements OnInit {
  @ViewChild('search', {static: true}) searchTerm: ElementRef;

  items: IItem[] = []
  itemTypes: IItemType[] = []
  categories: ICategory[] = []

  itemPagination: Pagination;  
  itemTypPagination: Pagination;  
  categoryPagination: Pagination;  

  totalCount: number;

 itemParams = new ItemParams();
 itemTypeParams = new ItemTypeParams();
 categoryParams = new CategoryParams();



  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ]
  
  constructor(private itemService: ItemService, private itemTypeService: ItemTypeService, 
    private toasterService: ToasterService,
    private categoryService: CategoryService) { }

  ngOnInit() {
    this.initParams();
    this.getItems();
    this.getItemTypes();
    this.getCategories();
  }


  getItems() {
    this.itemService.getItemsForCustomer(this.itemParams, this.itemPagination.currentPage, this.itemPagination.itemsPerPage)
      .subscribe({
      next: (res) => {
        if (res) {
          this.items = res.result;
          this.totalCount = res.pagination.totalItems;
          this.itemPagination = res.pagination;
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

  getItemTypes() {
    this.itemTypeService.getItemTypesForCustomer(this.itemTypeParams.categoryId, this.itemTypPagination.currentPage, this.itemTypPagination.itemsPerPage, this.itemTypeParams).subscribe({
      next: (res) => {
        if (res) {
          this.itemTypes = res.result
          this.itemTypPagination = res.pagination;
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

  getCategories() {
    this.categoryService.getCategoriesForCustomer(this.categoryPagination.currentPage, this.categoryPagination.itemsPerPage, this.categoryParams).subscribe({
      next: (res) => {
        if (res) {
          this.categories = res.result;
          this.categoryPagination = res.pagination;
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

  onItemTypeSelected(itemTypeId: number) {
    this.itemParams.itemTypeId = itemTypeId;
    this.itemPagination.currentPage = 1;
    this.getItems()
  }

  onCategorySelected(categoryId: number) {
    this.itemParams.categoryId = categoryId;
    this.itemPagination.currentPage = 1;

    this.getItems()
  }

  onSortSelected(sort: any) {    
    this.itemParams.sort = (sort.target as HTMLInputElement).value;
    this.getItems();
  }

  getNextPage(currentPage: number) {
    if (this.itemPagination.currentPage != currentPage) {
      this.itemPagination.currentPage = currentPage;
      this.getItems();
    }
  }

  onSearch() {
    this.itemParams.search = this.searchTerm.nativeElement.value;
    this.itemPagination.currentPage = 1;
    this.getItems();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.itemParams = new ItemParams();
    this.getItems();
  }

  initParams() {    
    this.itemPagination = QUERY;
    this.categoryPagination = QUERY;
    this.itemTypPagination = QUERY;
   }

}
