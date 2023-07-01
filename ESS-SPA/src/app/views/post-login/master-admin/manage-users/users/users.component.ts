import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import {IUser} from '../../../../../entities/models/user'
import { UserService } from 'src/app/shared/services/user-service/user.service';
import { Pagination } from 'src/app/entities/models/pagination';
import { error } from 'console';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { QUERY } from 'src/app/constants/app.constant';
import { EntityStatus } from 'src/app/entities/models/entityStatus';
import { AccountType } from 'src/app/entities/models/accountType';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: IUser[] = [];
  userParams: any = {};
  pagination: Pagination;
  filterStatus: { id: number; name: string }[] = [];
  accountTypes: { id: number; name: string }[] = [];

  test = 'success';
  constructor(private route: ActivatedRoute, 
    private toasterService: ToasterService,
    private userService: UserService) { }

  ngOnInit() {
    this.initUserParams();
    this.initFilter(EntityStatus);
    this.initAccountType(AccountType);
    this.getUsers(this.userParams.status);
  }

  // get users
  getUsers(currentStatus?: number, accounType?: number) {
    this.userParams.status = currentStatus === undefined ? this.userParams.status : currentStatus;
    this.userParams.accountType = accounType === undefined ? this.userParams.accountType : accounType;
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams).subscribe({
      next: ((res) => {
        this.users = res.result
        this.pagination = res.pagination;

      }), error: ((error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      })
    })
  }

  onStatusChange(data: any){
    this.getUsers(data.target.value);
  }

  onAccountTypeChange(data: any) {
    this.userParams.accountType = data.target.value;
    this.getUsers(this.userParams.status);
  }

  initFilter(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.filterStatus.push({ id: <any>status[n], name: n });
      }
    }
  }

  initAccountType(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.accountTypes.push({ id: <any>status[n], name: n });
      }
    }
  }

  onApproveUser(user: IUser) {

    let result = confirm(
      `Are you sure you want to approve this user?`
    );

    if (!result) {
      return;
    }

    this.userService.approveUser(user).subscribe({
      next: (res) => {
        if (res){
          this.users.splice(this.users.findIndex(c => c.id === res.id), 1);
          this.toasterService.showSuccess('SUCCESS', 'User account activated successfully');
        }
      },
      error: (error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      }
    });
  }

  onRejectUser(user: IUser) {
    let result = confirm(
      `Are you sure you want to reject this user?`
    );

    if (!result) {
      return;
    }

    this.userService.rejectUser(user).subscribe({
      next: (res) => {
        if (res){
          this.users.splice(this.users.findIndex(c => c.id === res.id), 1);
          this.toasterService.showSuccess('SUCCESS', 'User account rejected successfully');
        }
      },
      error: (error: ErrorResponse) => {
        this.toasterService.showError(error.title, error.message);
      }
    })
  }

  onEnableDisableUser(user: IUser, status: boolean) {
    var res = (status) ? 'enable' : 'disable';
    let result = confirm(
      `Are you sure you want to ${res} this user?`
    );

    if (!result) {
      return;
    }

    this.userService.onEnableDisableUser(user.id, status).subscribe({
      next: ((res) => {
         this.users.splice(this.users.findIndex(c => c.id === res.id), 1);
         if (res.status.toLocaleLowerCase() === 'active'){
          this.toasterService.showInfo('SUCCESS', 'User activated successfully'); 

         }
         this.toasterService.showInfo('SUCCESS', 'User disabled successfully'); 
      }),
      error: ((error: ErrorResponse) => {
       this.toasterService.showError(error.title, error.message);
      })
    });
  }

 
  private initUserParams() {    
   this.pagination = QUERY;
   this.userParams.searchString = '';
  this.userParams.status = EntityStatus.Pending;
   this.userParams.accountType = AccountType.Individual;
  }

}
