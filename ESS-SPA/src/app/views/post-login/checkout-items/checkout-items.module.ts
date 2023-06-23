import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule, GridModule, WidgetModule, TableModule, FormModule } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from '../../../shared/modules/custom.module';
import { CheckoutItemsComponent } from './checkout-items.component';
import {CheckoutItemsRoutingModule} from './checkout-items-routing.module';
import { FormsModule } from '@angular/forms';
import {ReviewBasketComponent} from './review-basket/review-basket.component';

@NgModule({
    imports: [
        CardModule,
        CommonModule,
        GridModule,
        WidgetModule,
        IconModule,
       CustomModule,
       TableModule,
       FormsModule,
       FormModule,
       CheckoutItemsRoutingModule,
      //
    //    CheckOutItemsModule
    ],
    declarations: [
        CheckoutItemsComponent,
        ReviewBasketComponent
    ],
    entryComponents: [
        ReviewBasketComponent
    ],
  
    providers: [
        
    ]
  })
  export class CheckOutItemsModule { }