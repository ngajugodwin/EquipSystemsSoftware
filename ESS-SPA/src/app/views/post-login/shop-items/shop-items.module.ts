import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopItemsRoutingModule } from './shop-items-routing.module';
import { ShopItemsComponent } from './shop-items.component';
import { CardModule, GridModule, WidgetModule } from '@coreui/angular';
import { WidgetsModule } from '../../widgets/widgets.module';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { IconModule } from '@coreui/icons-angular';
import {ProductItemComponent} from './product-item/product-item.component';
import { CustomModule } from 'src/app/shared/modules/custom.module';

@NgModule({
    imports: [
        CardModule,
        ShopItemsRoutingModule,
        CommonModule,
        GridModule,
        WidgetModule,
        CardModule,
        IconModule,
        CustomModule
    ],
    declarations: [
        ShopItemsComponent,
        ProductItemComponent
    ],
    entryComponents: [
    ],
  
    providers: [
        
    ]
  })
  export class ShopItemsModule { }