import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BasketComponent } from './basket.component';

const routes: Routes = [

    {path: '', component: BasketComponent, data: {title: 'Basket Items'}},
    // {path: ':id', component: ProductDetailsComponent, data: {title: 'Item Details'}},
    { path: '**', redirectTo: '/404' }
    
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class BasketRoutingModule {}