import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CheckoutItemsComponent } from './checkout-items.component';
import { PaymentComponent } from './payment/payment.component';

const routes: Routes = [

    {path: '', component: CheckoutItemsComponent, data: {title: 'Checkout'}},
    {path: 'payment', component: PaymentComponent, data: {title: 'Checkout/Payment'}},
    { path: '**', redirectTo: '/404' }
    
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class CheckoutItemsRoutingModule {}