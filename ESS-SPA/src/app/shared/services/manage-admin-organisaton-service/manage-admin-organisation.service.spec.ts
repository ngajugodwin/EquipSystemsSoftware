/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ManageAdminOrganisationService } from './manage-admin-organisation.service';

describe('Service: OrganisationUser', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ManageAdminOrganisationService]
    });
  });

  it('should ...', inject([ManageAdminOrganisationService], (service: ManageAdminOrganisationService) => {
    expect(service).toBeTruthy();
  }));
});
