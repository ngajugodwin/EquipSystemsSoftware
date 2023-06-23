import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrder } from 'src/app/entities/models/order';
import { Pagination } from 'src/app/entities/models/pagination';
import { CheckoutService } from 'src/app/shared/services/checkout-service/checkout.service';

@Component({
  selector: 'app-booking-details',
  templateUrl: './booking-details.component.html',
  styleUrls: ['./booking-details.component.css']
})
export class BookingDetailsComponent implements OnInit {
  order: IOrder;
  
  constructor(private checkoutService: CheckoutService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getBookingDetails();
  }

  getBookingDetails() {
    this.checkoutService.getOrderDetails(Number(this.route.snapshot.paramMap.get('id'))).subscribe({
      next: (res) => {
        if (res) {
          this.order = res
        }
      },
      error: (err: ErrorResponse) => {
        console.log(err);
      }
    })
  }

}
