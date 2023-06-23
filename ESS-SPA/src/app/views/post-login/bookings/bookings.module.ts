import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule, GridModule, WidgetModule, TableModule, AvatarModule,BadgeModule, DropdownModule } from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from '../../../shared/modules/custom.module';
import { BookingsRoutingModule } from './bookings-routing.module';
import { BookingsComponent } from './bookings.component';
import { BookingDetailsComponent } from './booking-details/booking-details.component';

@NgModule({
    imports: [
        CardModule,
        BookingsRoutingModule,
        CommonModule,
        GridModule,
        WidgetModule,
        CardModule,
        IconModule,
        CustomModule,
        AvatarModule,
        TableModule,
        DropdownModule,
        BadgeModule
    ],
    declarations: [
        BookingsComponent,
        BookingDetailsComponent
    ],
    entryComponents: [
    ],
  
    providers: [
        
    ]
  })
  export class BookingsModule { }