import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule, GridModule, WidgetModule, TableModule, FormModule } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from '../../../shared/modules/custom.module';
import { CheckoutItemsComponent } from './checkout-items.component';
import {CheckoutItemsRoutingModule} from './checkout-items-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {ReviewBasketComponent} from './review-basket/review-basket.component';
import {PaymentComponent} from './payment/payment.component';

@NgModule({
    imports: [
        CommonModule,
        GridModule,
        WidgetModule,
        IconModule,
       CustomModule,
       CardModule,
       TableModule,
       FormsModule,
       ReactiveFormsModule,
       FormModule,
       CheckoutItemsRoutingModule,
    //    PaymentComponent
      //
    //    CheckOutItemsModule
    ],
    declarations: [
        CheckoutItemsComponent,
        ReviewBasketComponent,
        PaymentComponent
    ],
    entryComponents: [
        ReviewBasketComponent,
        PaymentComponent
    ],
  
    providers: [
        
    ]
  })
  export class CheckOutItemsModule { }