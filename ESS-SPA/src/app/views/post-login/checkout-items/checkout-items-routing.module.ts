import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CheckoutItemsComponent } from './checkout-items.component';

const routes: Routes = [

    {path: '', component: CheckoutItemsComponent, data: {title: 'Checkout'}},
    { path: '**', redirectTo: '/404' }
    
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class CheckoutItemsRoutingModule {}