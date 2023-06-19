import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ShopItemsComponent} from './shop-items.component'

const routes: Routes = [

    {path: '', component: ShopItemsComponent, data: {title: 'Shop Items'}}
    // {
    //   path: '',
    //   data: {
    //     title: 'Shop Items'
    //   },
      
    //   children: [
    //     {
    //       path: '',
    //       redirectTo: 'shop-items'
    //     },
    //     {
    //       path: 'shop-items',
    //       component: ShopItemsComponent,
    //       data: {
    //         title: 'Shop Items'
    //       }
    //     },
    //     { path: '**', redirectTo: '/404' }
    //   ]
    // }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class ShopItemsRoutingModule {}