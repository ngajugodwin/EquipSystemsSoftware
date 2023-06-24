import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { SelfService } from 'src/app/shared/services/self-service/self.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  passwordForm: FormGroup;
  isSaving = false;
  title = 'Change Password';
  
  constructor(private fb: FormBuilder, 
      private authService: AuthService,
      private selfService: SelfService) { }

  ngOnInit() {
    this.createPasswordForm();
  }

  reset() {
    this.passwordForm.reset();
  }

  createPasswordForm() {
    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, {validator: [this.passwordMatchValidator]} as AbstractControlOptions)
  }

  save() {
    if (this.passwordForm.valid) {
      this.isSaving = true;
      const changePasswordData = {
        oldPassword: this.passwordForm.value.currentPassword,
        newPassword: this.passwordForm.value.newPassword
      };

      this.selfService.changePassword(this.authService.getCurrentUser().id, changePasswordData).subscribe({
        next: (user: IUser) => {
          if (user) {
            console.log('Password updated successfully');
            this.reset();
          }
        },
        error: (err: ErrorResponse) => {
          console.log(err);
        }
      })

    }
  }

  private passwordMatchValidator(f: FormGroup) {
    return f.get('newPassword')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }
}
