import { Component, Input, OnInit } from '@angular/core';
import { IUser } from 'src/app/entities/models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  @Input() user: IUser;
  
  constructor() { }

  ngOnInit() {
  }

}
