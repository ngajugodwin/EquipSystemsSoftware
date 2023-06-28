import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { error } from 'console';
import { AccountType } from 'src/app/entities/models/accountType';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { IUser } from 'src/app/entities/models/user';
import { ToasterService } from 'src/app/shared/services/toaster-service/toaster.service';
import { UserService } from 'src/app/shared/services/user-service/user.service';

@Component({
  selector: 'app-individual-account',
  templateUrl: './individual-account.component.html',
  styleUrls: ['./individual-account.component.css']
})
export class IndividualAccountComponent implements OnInit {
  selectedFile: any;
  userForm: FormGroup;
  user: IUser;
  
  constructor(private fb: FormBuilder,
    private router: Router,
     private userService: UserService, private toasterService: ToasterService) { }

  ngOnInit() {
    this.initUserForm();
  }

  save() {

    if (this.selectedFile === null || this.selectedFile === undefined) {
      alert('Please select a file');
      return;
    }  
    
    if (!this.selectedFile.name.includes('.jpg', 0)) {
      alert('Only jpg pictures are supported');
      return;
    }

    if (this.userForm.valid) {
      const user: IUser = Object.assign({}, this.userForm.value);
      user.accountType = AccountType.Individual;      

      this.userService.createUserAccountV2(user, this.selectedFile).subscribe({
        next: ((res) => {
          if (res) {
            this.toasterService.showSuccess('SUCCESS', "Account created and pending activation");
            this.clear();
            this.router.navigate(['/login']);
          } else {
            this.toasterService.showError('ERROR', "Unable to create your account");

          }
        
        }),
        error: ((error: ErrorResponse) => {
          this.toasterService.showError(error.title, error.message);
        })
      });
  }}

  onFileChanged(event: any) {
    this.selectedFile = event.target.files[0]
  }


  private initUserForm() {
    this.userForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'), Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      city: ['', Validators.required],
      file: ['', Validators.required],
      state: ['', Validators.required],
      street: ['', Validators.required],
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
