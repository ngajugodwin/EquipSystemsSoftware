import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { FILE_TYPE_FORMAT, QUERY } from 'src/app/constants/app.constant';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrder } from 'src/app/entities/models/order';
import { Pagination } from 'src/app/entities/models/pagination';
import { OrdersReportService } from 'src/app/shared/services/orders-report-service/orders-report.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  orders: IOrder[] = [];
  pagination: Pagination; 
  endDate: string;
  startDate: string;
  orderReportParams: any = {};
  bsValue = new Date();
  bsConfig: Partial<BsDatepickerConfig>;
  isButtonClicked = false;

  constructor(private ordersReportService: OrdersReportService, 
    private toasterService: ToasterService) { }

  ngOnInit() { 
    this.initOrderReportsParams();
    this.initDatePicker();
  }

  getReports() {

    if (this.orderReportParams.startDate === null || this.orderReportParams.endDate === null) {
      let msg = alert('You must select a date range to generate report');
      return;
    }

    if (this.orderReportParams.endDate < this.orderReportParams.startDate) {
      let msg = alert('End date cannot be greater than start date');
      return;
    }

    this.ordersReportService.getOrdersReport(this.pagination.currentPage, this.pagination.itemsPerPage, this.orderReportParams).subscribe({
      next: (res) => {
        if (res) {
          console.log(res);
          this.orders = res.result;
          this.pagination = res.pagination
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }
    })

  }

  onStartDateValueChange(value: any): void {
    this.orderReportParams.startDate = value;
    console.log(value);
  }

  onEndDateValueChange(value: any): void {
    this.orderReportParams.endDate = value;
  }

  getNextPage(currentPage: number) {
    this.pagination.currentPage = currentPage;
    this.getReports();
  }

  onExportReport() {
    if (this.orderReportParams.startDate === null || this.orderReportParams.endDate === null) {
      let msg = alert('You must select a date range to export a report');
      return;
    }

    if (this.orderReportParams.endDate < this.orderReportParams.startDate) {
      let msg = alert('End date cannot be greater than start date');
      return;
    }


   this.onOrderDownload();
  }

  onOrderDownload() {
    this.isButtonClicked = true;
    this.ordersReportService.downloadOrderReport(this.orderReportParams).subscribe({
      next: (res) => {
          if (res) {
        const newBlob = new Blob([res], { type: FILE_TYPE_FORMAT.EXCEL });
        const data = window.URL.createObjectURL(newBlob);
        const link = document.createElement('a');
        link.href = data;
        link.download = this.generateFileName();
        link.click();
        this.toasterService.showInfo('SUCCESS', 'Download successful');
        this.isButtonClicked = false;
      }
      },
      error: (err: ErrorResponse) => {
        this.toasterService.showError(err.title, err.message);
      }   
    })
  }


  private generateFileName(): string {
    const today = new Date().toLocaleDateString();
    const date = today.split('/').reverse().join('');
    const value = `${date}_OrderReport.xlsx`;

    return value;
  }


  
  initOrderReportsParams() {    
    this.pagination = QUERY;
    this.orderReportParams.startDate = null;
    this.orderReportParams.endDate = null;
    this.orderReportParams.searchString = '';
  }

  private initDatePicker() {
    this.bsConfig = {
      containerClass: 'theme-dark-blue'
    };
  }

}
