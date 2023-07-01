import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrder } from 'src/app/entities/models/order';
import { CheckoutService } from 'src/app/shared/services/checkout-service/checkout.service';
import { OrderService } from 'src/app/shared/services/order-service/order.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})
export class DetailsComponent implements OnInit {
  order: IOrder;
  
  constructor(private orderService: OrderService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.getBookingDetails();
  }

  getBookingDetails() {
    this.orderService.getOrderDetails(Number(this.route.snapshot.paramMap.get('id'))).subscribe({
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
