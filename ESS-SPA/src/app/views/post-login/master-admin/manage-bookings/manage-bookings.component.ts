import { Component, OnInit } from '@angular/core';
import { QUERY } from 'src/app/constants/app.constant';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrder, OrderBookingStatus, OrderPaymentStatus } from 'src/app/entities/models/order';
import { Pagination } from 'src/app/entities/models/pagination';
import { OrderService } from 'src/app/shared/services/order-service/order.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-manage-bookings',
  templateUrl: './manage-bookings.component.html',
  styleUrls: ['./manage-bookings.component.css']
})
export class ManageBookingsComponent implements OnInit {
  orders: IOrder[] = [];
  pagination: Pagination; 
  orderParams: any = {};
  paymentStatus: { id: number; name: string }[] = [];
  bookingStatus: { id: number; name: string }[] = [];


  constructor(private orderService: OrderService, private toaserService: ToasterService) { }

  ngOnInit() {
    this.initPaymentFilter(OrderPaymentStatus);
    this.initBookingStatusFilter(OrderBookingStatus);
    this.initOrdersParams();
    this.getAllOrders();
  }

  getAllOrders() {
    this.orderService.getOrdersForModeration(this.pagination.currentPage, this.pagination.itemsPerPage, this.orderParams).subscribe({
      next: (res) => {
        if (res) {
          this.orders = res.result;
          this.pagination = res.pagination
        }
      },
      error: (err: ErrorResponse) => {
        this.toaserService.showError(err.title, err.message);
      }
    })
  }

  onApproveOrder(order: IOrder){
    let result = confirm(
      `Are you sure you want to approve this booking?`
    );

    if (!result) {
      return;
    }

    this.orderService.approveOrder(order.id).subscribe({
      next: (res)=> {
        if (res) {
          this.orders.splice(
            this.orders.findIndex((x) => x.id === res.id),
            1
          );
          this.toaserService.showSuccess('SUCCESS', 'Selected booking has been approved');
        }
      },
      error: (err: ErrorResponse) => {
        this.toaserService.showError(err.title, err.message);

      }
    });


  }

  onDenyOrder(order: IOrder) {
    let result = confirm(
      `Are you sure you want to reject this booking?`
    );

    if (!result) {
      return;
    }

    this.orderService.rejectOrder(order.id).subscribe({
      next: (res)=> {
        if (res) {
          this.orders.splice(
            this.orders.findIndex((x) => x.id === res.id),
            1
          );
          this.toaserService.showSuccess('SUCCESS', 'Selected booking has been rejected');
        }
      },
      error: (err: ErrorResponse) => {
        this.toaserService.showError(err.title, err.message);

      }
    });
  }

  onCloseOrder(order: IOrder) {
    let result = confirm(
      `Are you sure you want to close this booking?`
    );

    if (!result) {
      return;
    }

    
    this.orderService.closeOrder(order.id).subscribe({
      next: (res)=> {
        if (res) {         
          this.orders.splice(
            this.orders.findIndex((x) => x.id === res.id),
            1
          );
          this.toaserService.showSuccess('SUCCESS', 'Selected booking has been closed');
        }
      },
      error: (err: ErrorResponse) => {
        this.toaserService.showError(err.title, err.message);

      }
    });

  }

  initOrdersParams() {    
    this.pagination = QUERY;
    this.orderParams.searchString = '';
    this.orderParams.status = OrderPaymentStatus.Pending;
    this.orderParams.bookingStatus = OrderBookingStatus.Pending;
  }

  onPaymentStatusChange(data: any) {
    this.orderParams.status = data.target.value
    this.getAllOrders();
  }

  onBookingStatusChange(data: any) {
    this.orderParams.bookingStatus = data.target.value
    this.getAllOrders();
  }

  getNextPage(currentPage: number) {
    if (this.pagination.currentPage != currentPage) {
      this.pagination.currentPage = currentPage;
      this.getAllOrders();
    }
  }

  private initPaymentFilter(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.paymentStatus.push({ id: <any>status[n], name: n });
      }
    }
  }

  private initBookingStatusFilter(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.bookingStatus.push({ id: <any>status[n], name: n });
      }
    }
  }


}
