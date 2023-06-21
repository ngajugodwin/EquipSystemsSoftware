import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule, GridModule, WidgetModule } from '@coreui/angular';
import { WidgetsModule } from '../../widgets/widgets.module';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from '../../../shared/modules/custom.module';
import {BasketComponent} from './basket.component';
import {BasketRoutingModule} from './basket-routing.module';

@NgModule({
    imports: [
        CardModule,
        BasketRoutingModule,
        CommonModule,
        GridModule,
        WidgetModule,
        CardModule,
        IconModule,
        CustomModule
    ],
    declarations: [
        BasketComponent
    ],
    entryComponents: [
    ],
  
    providers: [
        
    ]
  })
  export class BasketModule { }