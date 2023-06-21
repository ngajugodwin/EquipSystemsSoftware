import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule, GridModule, WidgetModule } from '@coreui/angular';
import { WidgetsModule } from '../../widgets/widgets.module';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from '../../../shared/modules/custom.module';
import { CheckoutRoutingModule } from './checkout-routing.module';
import { CheckoutComponent } from './checkout.component';

@NgModule({
    imports: [
        CardModule,
        CheckoutRoutingModule,
        CommonModule,
        GridModule,
        WidgetModule,
        CardModule,
        IconModule,
        CustomModule
    ],
    declarations: [
        CheckoutComponent
    ],
    entryComponents: [
    ],
  
    providers: [
        
    ]
  })
  export class CheckOutModule { }