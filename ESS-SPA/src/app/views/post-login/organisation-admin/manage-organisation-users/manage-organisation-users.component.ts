import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { QUERY } from 'src/app/constants/app.constant';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { Pagination } from 'src/app/entities/models/pagination';
import { IUser } from 'src/app/entities/models/user';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { ManageAdminOrganisationService } from 'src/app/shared/services/manage-admin-organisaton-service/manage-admin-organisation.service';

@Component({
  selector: 'app-manage-organisation-users',
  templateUrl: './manage-organisation-users.component.html',
  styleUrls: ['./manage-organisation-users.component.css']
})
export class ManageOrganisationUsersComponent implements OnInit {
  filterStatus: { id: number; name: string }[] = [];
  userParams: any = {};
  pagination: Pagination;  
  users: IUser[] = [];
  userState = false;

  constructor(private authService: AuthService, private router: Router, private manageAdminOrganisationService: ManageAdminOrganisationService) { }

  ngOnInit() {
    this.initUserParams()
    this.initFilter(EntityStatus);
    this.getOrganisationUsers(this.userParams.status);

    this.authService.getOrganisationId();
  }

  getOrganisationUsers(currentStatus?: number) {
    this.userParams.status = currentStatus === undefined ? this.userParams.status : currentStatus;
    this.manageAdminOrganisationService
      .getOrganisationUsers(this.authService.getOrganisationId(), this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams).subscribe({
        next: (res) => {
          console.log(res);
          this.users = res.result;
          this.pagination = res.pagination;
        },
        error: (error: ErrorResponse) => {
          console.log(error); //TOD: show error toaster message
        }
      });
  }

  onStatusChange(data: any){
    this.getOrganisationUsers(data.target.value);
  }

  onEditUser(user: IUser) {
    this.router.navigate([`/organisation-admin/manage-organisation-users/edit/${user.id}`]);
  }

  onChangePassword(user: IUser) {
    this.router.navigate([`/organisation-admin/manage-organisation-users/change-password/${user.id}`]);
  }

  onUserStatusChange(userId: number, status: boolean) {
    var res = (status) ? 'enable' : 'disable';
    let result = confirm(
      `Are you sure you want to ${res} this user?`
    );

    if (!result) {
      return;
    }

    this.manageAdminOrganisationService.activateOrDisableUser(this.authService.getOrganisationId(), userId, status).subscribe({
      next: ((res) => {
         this.users.splice(this.users.findIndex(c => c.id === res.id), 1);
         if (res.status.toLocaleLowerCase() === 'active'){
          console.log('User enabled successfully'); //TODO: show success toaster
         }
         console.log('User disabled successfully'); //TODO: show success toaster
      }),
      error: ((error: ErrorResponse) => {
        console.log(error); //TODO: show error toaster
      })
    });
  }


  private initUserParams() {    
    this.pagination = QUERY;
    this.userParams.searchString = '';
   this.userParams.status = EntityStatus.Active;
   }

   
  initFilter(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.filterStatus.push({ id: <any>status[n], name: n });
        this.filterStatus = this.filterStatus.filter(x => x.name.toLowerCase() !== 'pending');
      }
    }
  }

}
