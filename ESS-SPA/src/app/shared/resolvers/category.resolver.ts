import { Observable, empty, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { ICategory } from '../../entities/models/category';
import { ManageAdminOrganisationService } from '../services/manage-admin-organisaton-service/manage-admin-organisation.service';
import { CategoryService } from '../services/category-service/category.service';


@Injectable()

export class CategoryResolver implements Resolve<ICategory> {

    constructor(private categoryService: CategoryService,
       private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): ICategory | Observable<ICategory> | Promise<ICategory> {
        return this.categoryService.getCategory(route.params['id']).pipe(
            catchError((err) => {
                console.log(err);
                this.router.navigate(['/master-admin/manage-categories']);
                return of(err);
            })
           
        )
    }
}