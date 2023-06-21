import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CheckoutComponent } from './checkout.component';

const routes: Routes = [

    {path: '', component: CheckoutComponent, data: {title: 'Checkout'}},
    { path: '**', redirectTo: '/404' }
    
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class CheckoutRoutingModule {}