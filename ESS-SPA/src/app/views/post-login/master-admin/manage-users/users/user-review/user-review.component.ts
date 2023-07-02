import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';
import { UserService } from 'src/app/shared/services/user-service/user.service';

@Component({
  selector: 'app-user-review',
  templateUrl: './user-review.component.html',
  styleUrls: ['./user-review.component.css']
})
export class UserReviewComponent implements OnInit {
  user: IUser;

  constructor(private userService: UserService, 
    private route: ActivatedRoute, 
    private toasterservice: ToasterService) { }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.userService.getUser(Number(this.route.snapshot.paramMap.get('id'))).subscribe({
      next: (res) => {
        if (res) {
          this.user = res
        }
      },
      error: (err: ErrorResponse) => {
        this.toasterservice.showError(err.title, err.message);
      }
    })
  }
}
