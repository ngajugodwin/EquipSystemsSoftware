import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import {IUser} from '../../../../../entities/models/user'

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: IUser[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
  }

  getUsers() {
    this.route.data.pipe(map((res) => {
      console.log(res);
    }),
    );
  }

}
