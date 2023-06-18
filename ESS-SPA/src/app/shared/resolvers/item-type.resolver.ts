import { Observable, empty, of, pipe } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError, map } from 'rxjs/operators';
import { ICategory } from '../../entities/models/category';
import { ManageAdminOrganisationService } from '../services/manage-admin-organisaton-service/manage-admin-organisation.service';
import { CategoryService } from '../services/category-service/category.service';
import {ItemTypeService} from '../services/itemTypes-service/item-type.service';
import { IItemType } from 'src/app/entities/models/itemType';


@Injectable()

export class ItemTypeResolver implements Resolve<IItemType> {
    constructor(private itemTypeService: ItemTypeService,
        private categoryService: CategoryService,
       private router: Router) {}
    cat: ICategory;
    resolve(route: ActivatedRouteSnapshot): IItemType | Observable<IItemType> | Promise<IItemType> {
        console.log(route.params['id']);
        this.getCategory();
        return this.itemTypeService.getItemType(this.cat.id, route.params['id']).pipe(
            catchError((err) => {
                this.router.navigate(['/master-admin/manage-categories']);
                return of(err);
            })
           
        )
    }    

    getCategory () {
        this.categoryService.retrieveCurrentCategory().subscribe({
            next: (res: ICategory) => {
                this.cat = res;
            }
        })
            
    }
    
}