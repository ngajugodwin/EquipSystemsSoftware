import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {BookingsComponent} from './bookings.component';
import {BookingDetailsComponent} from './booking-details/booking-details.component';

const routes: Routes = [

    {path: '', component: BookingsComponent, data: {title: 'My Bookings'}},
    {path: ':id', component: BookingDetailsComponent, data: {title: 'My Booking Details'}},
    // {path: ':id', component: ProductDetailsComponent, data: {title: 'Item Details'}},
    { path: '**', redirectTo: '/404' }
    
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class BookingsRoutingModule {}