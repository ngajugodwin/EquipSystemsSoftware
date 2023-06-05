import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { error } from 'console';
import { AccountType } from 'src/app/entities/models/accountType';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { UserService } from 'src/app/shared/services/user-service/user.service';

@Component({
  selector: 'app-individual-account',
  templateUrl: './individual-account.component.html',
  styleUrls: ['./individual-account.component.css']
})
export class IndividualAccountComponent implements OnInit {
  userForm: FormGroup;
  user: IUser;
  
  constructor(private fb: FormBuilder, private userService: UserService) { }

  ngOnInit() {
    this.initUserForm();
  }

  save() {
    if (this.userForm.valid) {
      const user: IUser = Object.assign({}, this.userForm.value);
      user.accountType = AccountType.Individual;
      this.userService.createUserAccount(user).subscribe({
        next: (() => {
          console.log("New User account created successfully")
          this.clear();
        }),
        error: ((error: ErrorResponse) => {
          console.log(error);
        })
      });
  }}


  private initUserForm() {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'), Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validator: [this.passwordMatchValidator]} as AbstractControlOptions);    
  }

  private passwordMatchValidator(f: FormGroup) {
    return f.get('password')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }

  private clear() {
    this.userForm.reset();
  }
  
}
