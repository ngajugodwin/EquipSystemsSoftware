import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { QUERY } from 'src/app/constants/app.constant';
import { OrganisationParams } from 'src/app/entities/models/basequeryParams';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IOrganisation } from 'src/app/entities/models/organisation';
import { Pagination } from 'src/app/entities/models/pagination';
import { OrganisationService } from 'src/app/shared/services/organisation-service/organisation.service';

@Component({
  selector: 'app-manage-organisations',
  templateUrl: './manage-organisations.component.html',
  styleUrls: ['./manage-organisations.component.css']
})
export class ManageOrganisationsComponent implements OnInit {
  organisationParams: any = {};
  pagination: Pagination;  
  organisations: IOrganisation[] = [];


  constructor(private organisationService: OrganisationService) { }

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
      console.log('no');
      return;
    }
    this.organisationService.activateOrDisableOrganisation(organisationId, true).subscribe(
      (res: IOrganisation) => {
        if (res) {
          console.log(res); // show toaster
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
         console.log('Organisation rejected successfully'); // TODO show success toaster
      }),
      error: ((error: ErrorResponse) => {
        console.log(error); //TODO: show error toaster
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
          console.log('Organisation enabled successfully'); //TODO: show success toaster
         }
         console.log('Organisation disabled successfully'); //TODO: show success toaster
      }),
      error: ((error: ErrorResponse) => {
        console.log(error); //TODO: show error toaster
      })
    });
  }

}
