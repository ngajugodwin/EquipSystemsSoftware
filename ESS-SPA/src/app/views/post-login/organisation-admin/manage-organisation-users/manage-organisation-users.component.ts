import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/entities/models/pagination';
import { IUser } from 'src/app/entities/models/user';

@Component({
  selector: 'app-manage-organisation-users',
  templateUrl: './manage-organisation-users.component.html',
  styleUrls: ['./manage-organisation-users.component.css']
})
export class ManageOrganisationUsersComponent implements OnInit {
  userParams: any = {};
  pagination: Pagination;  
  users: IUser[] = [];

  constructor() { }

  ngOnInit() {
  }

  getOrganisationUsers(currentStatus?: number) {

  }

  onEditUser(user: IUser) {
    
  }

  onDisableUser(user: IUser) {
    
  }

}
