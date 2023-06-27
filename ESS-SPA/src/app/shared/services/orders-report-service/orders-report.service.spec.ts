/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { OrdersReportService } from './orders-report.service';

describe('Service: OrdersReport', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrdersReportService]
    });
  });

  it('should ...', inject([OrdersReportService], (service: OrdersReportService) => {
    expect(service).toBeTruthy();
  }));
});
