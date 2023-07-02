import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { QUERY } from 'src/app/constants/app.constant';
import { OrganisationParams } from 'src/app/entities/models/basequeryParams';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrganisation } from 'src/app/entities/models/organisation';
import { Pagination } from 'src/app/entities/models/pagination';
import { OrganisationService } from 'src/app/shared/services/organisation-service/organisation.service';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-manage-organisations',
  templateUrl: './manage-organisations.component.html',
  styleUrls: ['./manage-organisations.component.css']
})
export class ManageOrganisationsComponent implements OnInit {
  organisationParams: any = {};
  pagination: Pagination;  
  organisations: IOrganisation[] = [];
  

  constructor(private organisationService: OrganisationService, private toasterService: ToasterService) { }

  ngOnInit() {
    this.initOrganisationParams();
    this.getOrganisations(this.organisationParams.status);
  }

  getOrganisations(currentStatus?: number) {
    this.organisationParams.status = currentStatus === undefined ? this.organisationParams.status : currentStatus;
    this.organisationService.getOrganisations(this.pagination.currentPage, this.pagination.itemsPerPage, this.organisationParams).subscribe((res) => {
      this.organisations = res.result;
      this.pagination = res.pagination;
    });
  }

  onChangeOrganisationStatus(data: any) {
    const result = data.target.value;
    this.organisationParams.status = result;
  }


  initOrganisationParams() {    
    this.pagination = QUERY;
   this.organisationParams.searchString = '';
   this.organisationParams.status = EntityStatus.Pending;
  }

  onApproveOrganisation(organisationId: number) {
    let result = confirm(
      "Are you sure you want to approve this organisation?"
    );
    if (!result) {
      return;
    }
    this.organisationService.activateOrDisableOrganisation(organisationId, true).subscribe(
      (res: IOrganisation) => {
        if (res) {
          this.toasterService.showInfo('SUCCESS', 'Organisation user account approved');
          this.organisations.splice(this.organisations.findIndex(x => x.id === res.id), 1);
        }
      }
    )
  }

  onRejectOrganisation(organisationId: number) {
    let result = confirm(
      "Are you sure you want to reject this organisation?"
    );
    if (!result) {
      return;
    }

    this.organisationService.rejectOrganisation(organisationId).subscribe({
      next: ((res) => {
         this.organisations.splice(this.organisations.findIndex(c => c.id === res.id), 1);
         this.toasterService.showInfo('SUCCESS', 'Organisation rejected successfully');
      }),
      error: ((error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      })
    });
  }

  onEnableDisableOrganisation(organisationId: number, status: boolean) {
    var res = (status) ? 'enable' : 'disable';
    let result = confirm(
      `Are you sure you want to ${res} this organisation?`
    );

    if (!result) {
      return;
    }

    this.organisationService.activateOrDisableOrganisation(organisationId, status).subscribe({
      next: ((res) => {
         this.organisations.splice(this.organisations.findIndex(c => c.id === res.id), 1);
         if (res.status.toLocaleLowerCase() === 'active'){
          this.toasterService.showInfo('SUCCESS', 'Organisation enabled successfully'); 
         }
         this.toasterService.showInfo('SUCCESS', 'Organisation disabled successfully');
      }),
      error: ((error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      })
    });
  }

}
