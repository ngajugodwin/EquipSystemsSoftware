/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SelfService } from './self.service';

describe('Service: Self', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SelfService]
    });
  });

  it('should ...', inject([SelfService], (service: SelfService) => {
    expect(service).toBeTruthy();
  }));
});
