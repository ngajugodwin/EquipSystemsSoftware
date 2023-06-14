import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { ManageAdminOrganisationService } from 'src/app/shared/services/manage-admin-organisaton-service/manage-admin-organisation.service';

@Component({
  selector: 'app-change-user-password',
  templateUrl: './change-user-password.component.html',
  styleUrls: ['./change-user-password.component.css']
})
export class ChangeUserPasswordComponent implements OnInit {
  passwordForm: FormGroup;
  currentUser: IUser;
  isSaving: boolean;

  constructor(private activatedRoute: ActivatedRoute, 
      private fb: FormBuilder, 
      private router: Router,
      private manageAdminOrganisationService: ManageAdminOrganisationService) { }

  ngOnInit() {
    this.getCurrentUser();
    this.initPasswordForm();
  }

  initPasswordForm() {
    this.passwordForm = this.fb.group({
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, {validator: [this.passwordMatchValidator]} as AbstractControlOptions)
  }

  getCurrentUser() {
    this.activatedRoute.data.subscribe(data => {
      const user = data['user'];

      console.log(data);
      console.log(user);

      if (user) {
        this.currentUser = user;
      }
  
    })
  }

  save() {
    if (this.passwordForm.valid) {
      this.isSaving = true;
      const changePasswordData = {
        newPassword: this.passwordForm.value.newPassword
      };

      console.log(changePasswordData);
      console.log(this.currentUser);


      this.manageAdminOrganisationService.changeUserPassword(this.currentUser.organisationId, this.currentUser.id, changePasswordData).subscribe({
        next: (res: IUser) => {
          if (res) {
            console.log('Password Updated successfully');
            this.close();
          }
          this.isSaving = false;
        },
        error: (error: ErrorResponse) => {
          console.log(error);
          this.isSaving = false;
        }
      })
    }
  }

  close () {
    this.router.navigate([`/organisation-admin/manage-organisation-users`]);
  }

 
  private passwordMatchValidator(f: FormGroup) {
    return f.get('newPassword')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }


}
