import { Component, Input, OnInit } from '@angular/core';
import { error } from 'console';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { SelfService } from 'src/app/shared/services/self-service/self.service';

@Component({
  selector: 'app-self-service',
  templateUrl: './self-service.component.html',
  styleUrls: ['./self-service.component.css']
})
export class SelfServiceComponent implements OnInit {
currentUser: IUser;
  
  constructor(private authService: AuthService, private selfService: SelfService) { }

  ngOnInit() {
    this.getCurrentUser();
  }

  getCurrentUser() {
    this.selfService.getUserProfile(this.authService.getCurrentUser().id).subscribe({
      next: (user: IUser) => {
        if (user) {
          this.currentUser = user;
        }
      }, 
      error: (error: ErrorResponse) => {
        console.log(error);
      }
    });
  }

}
