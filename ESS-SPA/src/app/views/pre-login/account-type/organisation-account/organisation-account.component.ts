import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountType } from 'src/app/entities/models/accountType';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';
import { UserService } from 'src/app/shared/services/user-service/user.service';


@Component({
  selector: 'app-organisation-account',
  templateUrl: './organisation-account.component.html',
  styleUrls: ['./organisation-account.component.css']
})
export class OrganisationAccountComponent implements OnInit {
  selectedFile: any;
  userForm: FormGroup;
  user: IUser;

  constructor(private fb: FormBuilder, 
    private router: Router,
    private userService: UserService, private toasterService: ToasterService) { }

  ngOnInit() {
    this.initUserForm();
  }

  onFileChanged(event: any) {
    this.selectedFile = event.target.files[0]
  }

  save() {
    if (this.userForm.valid) {
      const user: IUser = Object.assign({}, this.userForm.value);
      user.accountType = AccountType.Organisation     
      user.organisation = Object.assign({}, this.userForm.controls['organisation'].value);
      
      this.userService.createOrganisationUserAccountV2(user, this.selectedFile).subscribe({
        next: ((res) => {
          if (res) {
            this.toasterService.showSuccess('SUCCESS', "New Organisation User account created successfully");
          this.router.navigate(['/login']);
          this.clear();
          }
        }),
        error: ((error: ErrorResponse) => {
          this.toasterService.showError(error.title, error.message);
        })
      });
     }
  }

  private initUserForm() {
    this.userForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'), Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      file: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      street: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required],
      organisation: this.fb.group({
        name: ['', Validators.required],
        registrationNumber: ['', Validators.required],
        address: ['', Validators.required],
        dateOfIncorporation: ['', Validators.required]
      })
    }, {validator: [this.passwordMatchValidator]} as AbstractControlOptions);    
  }


  private passwordMatchValidator(f: FormGroup) {
    return f.get('password')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }

  private clear() {
    this.userForm.reset();
  }

}
