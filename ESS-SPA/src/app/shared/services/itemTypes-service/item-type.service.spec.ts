/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ItemTypeService } from './item-type.service';

describe('Service: ItemType', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ItemTypeService]
    });
  });

  it('should ...', inject([ItemTypeService], (service: ItemTypeService) => {
    expect(service).toBeTruthy();
  }));
});
