import { Component, OnInit } from '@angular/core';
import { QUERY } from 'src/app/constants/app.constant';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrder } from 'src/app/entities/models/order';
import { Pagination } from 'src/app/entities/models/pagination';
import { CheckoutService } from 'src/app/shared/services/checkout-service/checkout.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.css']
})
export class BookingsComponent implements OnInit {
  orders: IOrder[] = [];
  orderParams: any = {};
  pagination: Pagination;  

  constructor(private checkOutService: CheckoutService, private toasterService: ToasterService) { }

  ngOnInit() {
    this.initOrderParams();
    this.getUserOrders();
  }

  getNextPage(currentPage: number) {
    if (this.pagination.currentPage != currentPage) {
      this.pagination.currentPage = currentPage;
      this.getUserOrders();
    }
  }

  getUserOrders() {
    this.checkOutService.getOrdersForUser().subscribe({
      next: (res) => {
        if (res) {
          this.orders = res.result;
          this.pagination = res.pagination;
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })
  }

  
  initOrderParams() {    
    this.pagination = QUERY;
    this.orderParams.searchString = '';
   }

}
