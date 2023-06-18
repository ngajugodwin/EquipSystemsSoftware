import { Observable, empty, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError } from 'rxjs/operators';
import {ItemTypeService} from '../services/itemTypes-service/item-type.service';
import { IItemType } from 'src/app/entities/models/itemType';
import { IItem } from 'src/app/entities/models/item';
import { ItemService } from '../services/item-service/item.service';


@Injectable()

export class ItemResolver implements Resolve<IItem> {

    constructor(private itemService: ItemService,
        private itemTypeService: ItemTypeService,
       private router: Router) {}
       itemType: IItemType;

    resolve(route: ActivatedRouteSnapshot): IItem | Observable<IItem> | Promise<IItem> {
        this.getItemType();
        return this.itemService.getItem(this.itemType.id, route.params['id']).pipe(
            catchError((err) => {
                console.log(err);
                this.router.navigate(['/master-admin/manage-categories']);
                return of(err);
            })
           
        )
    }    

    getItemType () {
        this.itemTypeService.retrieveCurrentItemType().subscribe({
            next: (res: IItemType) => {
                this.itemType = res;
            }
        })
            
    }
}