import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { MASTER_ADMIN_URL } from 'src/app/constants/api.constant';
import { ICategory } from 'src/app/entities/models/category';
import { PaginationResult } from 'src/app/entities/models/pagination';
import {BehaviorSubject} from 'rxjs';

// import {Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  // private currentCategory: any;

  private currentCategory = new BehaviorSubject<any>(null);
  
constructor(private http: HttpClient) {
 }


setCurrentCategory(category: ICategory) {
  this.currentCategory.next(category);
}

retrieveCurrentCategory(): Observable<ICategory> {
  return this.currentCategory.asObservable();
}


getCategory(categoryId: number): Observable<ICategory> {
  return this.http.get<ICategory>(MASTER_ADMIN_URL.MANAGE_CATEGORIES + `/${categoryId}`);
}

createCategory(categoryToCreate: ICategory) {
  return this.http.post<ICategory>(MASTER_ADMIN_URL.MANAGE_CATEGORIES, categoryToCreate);
}

updateCategory(categoryId: number, categoryToUpdate: ICategory) {
  return this.http.put<ICategory>(MASTER_ADMIN_URL.MANAGE_CATEGORIES + `/${categoryId}`, categoryToUpdate);
}

activateOrDisableCategory(categoryId: number, newStatus: boolean): Observable<ICategory> {
  let params = new HttpParams();
  params = new HttpParams().set('categoryStatus', newStatus);
  return this.http.put<ICategory>(MASTER_ADMIN_URL.MANAGE_CATEGORIES + `/${categoryId}/setCategoryStatus`, {}, {params: params});
}

getCategories(page?: number, itemsPerPage?: number, categoryParams?: any)  {
  const paginatedResult: PaginationResult<ICategory[]> = new PaginationResult<ICategory[]>();

  let params = new HttpParams();

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if (categoryParams !=  null) {
    params = params.append('searchString', categoryParams.searchString);

    if (categoryParams.status !=  null) {
      params = params.append('status', categoryParams.status);
    }
  }

  return this.http.get<ICategory[]>(MASTER_ADMIN_URL.MANAGE_CATEGORIES, {observe: 'response', params})
  .pipe(
    map((response: HttpResponse<ICategory[]>) => {
      if (response.body) {
        paginatedResult.result = response.body;
      }
      if (response.headers.get('Pagination') != null) {
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') || '');
      }
      return paginatedResult;
    })
  );
}


}
