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
  constructor(private route: ActivatedRoute, private userService: UserService) { }

  ngOnInit() {
    this.initUserParams();
    this.initFilter(EntityStatus);
    this.initAccountType(AccountType);
    this.getUsers(this.userParams.status);
  }

  getUsers(currentStatus?: number, accounType?: number) {
    this.userParams.status = currentStatus === undefined ? this.userParams.status : currentStatus;
    this.userParams.accountType = accounType === undefined ? this.userParams.accountType : accounType;
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams).subscribe({
      next: ((res) => {
        console.log(res);
        this.users = res.result
        this.pagination = res.pagination;

      }), error: ((error: ErrorResponse) => {
        console.log(error); //TODO display error toaster
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
        this.filterStatus = this.filterStatus.filter(x => x.name.toLowerCase() !== 'pending');
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

  private initUserParams() {    
   this.pagination = QUERY;
   this.userParams.searchString = '';
  this.userParams.status = EntityStatus.Active;
   this.userParams.accountType = AccountType.Individual;
  //  this.userParams.accountTypeName = '';
  }

}
