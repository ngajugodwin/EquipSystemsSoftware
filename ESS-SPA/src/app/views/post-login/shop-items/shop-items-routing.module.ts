import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ShopItemsComponent} from './shop-items.component'
import { ProductDetailsComponent } from './product-details/product-details.component';

const routes: Routes = [

    {path: '', component: ShopItemsComponent, data: {title: 'Shop Items'}},
    {path: ':id', component: ProductDetailsComponent, data: {title: 'Item Details'}},
    { path: '**', redirectTo: '/404' }
    
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class ShopItemsRoutingModule {}