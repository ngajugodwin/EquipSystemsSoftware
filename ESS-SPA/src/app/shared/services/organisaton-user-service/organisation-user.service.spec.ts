/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { OrganisationUserService } from './organisation-user.service';

describe('Service: OrganisationUser', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrganisationUserService]
    });
  });

  it('should ...', inject([OrganisationUserService], (service: OrganisationUserService) => {
    expect(service).toBeTruthy();
  }));
});
