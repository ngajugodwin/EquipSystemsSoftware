import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule, GridModule, WidgetModule, TableModule } from '@coreui/angular';
import { WidgetsModule } from '../../widgets/widgets.module';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from '../../../shared/modules/custom.module';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { CheckoutComponent } from './checkout.component';
import {CheckoutAddressComponent} from './checkout-address/checkout-address.component';
import {CheckoutDeliveryComponent} from './checkout-delivery/checkout-delivery.component';
import {CheckoutPaymentComponent} from './checkout-payment/checkout-payment.component';
import {CheckoutReviewComponent} from './checkout-review/checkout-review.component';
import {CheckoutSucessComponent} from './checkout-sucess/checkout-sucess.component';
import {StepperComponent} from '../../../shared/components/stepper/stepper.component';
// import { AvatarModule, BadgeModule, ButtonModule, CardModule, DropdownModule, FormModule, GridModule, ModalModule, TableModule } from '@coreui/angular';
@NgModule({
    imports: [
        CardModule,
        CheckoutRoutingModule,
        CommonModule,
        GridModule,
        WidgetModule,
        CardModule,
        IconModule,
       CustomModule,
       TableModule
    ],
    declarations: [
        CheckoutComponent,
        CheckoutAddressComponent,
        CheckoutSucessComponent,
        CheckoutPaymentComponent,
        CheckoutDeliveryComponent,
        CheckoutReviewComponent,
    ],
    entryComponents: [
    ],
  
    providers: [
        
    ]
  })
  export class CheckOutModule { }