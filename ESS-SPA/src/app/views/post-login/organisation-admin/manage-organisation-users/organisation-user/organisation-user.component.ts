import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IUser } from 'src/app/entities/models/user';
import { ErrorResponse } from 'src/app/entities/models/errorResponse';
import { AuthService } from 'src/app/shared/services/auth-service/auth.service';
import { ManageAdminOrganisationService } from 'src/app/shared/services/manage-admin-organisaton-service/manage-admin-organisation.service';
import { RoleService } from 'src/app/shared/services/role-service/role.service';
import { filter, map } from 'rxjs';
import { AccountType } from 'src/app/entities/models/accountType';

@Component({
  selector: 'app-organisation-user',
  templateUrl: './organisation-user.component.html',
  styleUrls: ['./organisation-user.component.css']
})
export class OrganisationUserComponent implements OnInit {
  private currentOrganisationId: number;
  formTitle: string = 'New User';
  roles: any = [];
  rolesForDisplay: any = [];
  userForm: FormGroup;
  isEditUserMode = false;
  currentUser: IUser;
  
  constructor(private fb: FormBuilder, 
      private router: Router, 
      private roleService: RoleService,
      private manageAdminOrganisationService: ManageAdminOrganisationService,
      private activatedRoute: ActivatedRoute,
      private authService: AuthService) { }

  ngOnInit() {
    this.initUserForm();
    this.getRoles();
    this.currentOrganisationId = this.authService.getOrganisationId();
    console.log('orgId' + this.currentOrganisationId);
  }

  get userFormGroups () {
    return this.userForm.get('roles') as FormArray;
  }

  initUserForm() {
    this.userForm = this.fb.group({
      id: [null],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'), Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      roles: new FormArray([]),
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required],
    }, {validator: [this.passwordMatchValidator]} as AbstractControlOptions)
  }

  private passwordMatchValidator(f: FormGroup) {
    return f.get('password')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }

  getRoles() {
    this.roleService.getRoles().subscribe((res) => {
      this.roles = res;
      this.getUser();
    });

    // this.roleService.getRoles()
  }

  getUser() {
    this.activatedRoute.data.subscribe(data => {
    const user = data['user'];
    if (user) {
      this.formTitle = 'Edit User';
      this.isEditUserMode = true;
      this.disableControl();
      this.getUserRoles(user);
      this.assignValuesToControl(user);
      console.log('EDIT USER');

    } else {
      console.log('NEW USER');
      this.formTitle = 'New User';
      const availableRoles = this.roles;
      for (let i = 0; i < availableRoles.length; i++) {
        availableRoles[i].selected = false;
        this.rolesForDisplay.push(availableRoles[i]);
      }
      this.addUserRoles(this.rolesForDisplay);
    }
   });
  }

  getUserRoles(user: IUser) {
    const userRoles = user.userRoles;
    const availableRoles = this.roles;

    if (user) {
      for (let i = 0; i < availableRoles.length; i++) {
        let isMatch = false;
        for (let j = 0; j < userRoles.length; j++) {
          if (availableRoles[i].name === userRoles[j]) {
            isMatch = true;
            availableRoles[i].selected = true;
            this.rolesForDisplay.push(availableRoles[i]);
            break;
          }
        }
        if (!isMatch) {
          availableRoles[i].selected = false;
          this.rolesForDisplay.push(availableRoles[i]);
        }
      }
      this.addUserRoles(this.rolesForDisplay);
    }
  }

  addUserRoles(roles: any) {
    for (let i = 0; i < roles.length; i++) {
       const control = new FormControl(roles[i].selected);
       (<FormArray>this.userForm.get('roles')).push(control);
    }
   }

   assignValuesToControl(user: IUser) {
    this.userForm.patchValue({
      id: user.id,
      username: user.userName,
      email: user.email || '',
      firstName: user.firstName || '',
      lastName: user.lastName || '',
      userRoles: user.userRoles
    });
  }

  getSelectedRoleName() {
    const selectedRoleNames = this.userForm.value.roles
      .map((item: any, index : any) => item ? this.rolesForDisplay[index].name : null)
      .filter((items : any) => items !== null);

      return selectedRoleNames;
  }

  save() {
    if(this.getSelectedRoleName().length == 0) {
      let res = confirm(`Please select at least one role for this user?`);
    }

    if (this.userForm.valid) {
     const result = this.getSelectedRoleName();
     this.userForm.value.roles = [];
     this.userForm.value.roles = result;
     const user: IUser = Object.assign({}, this.userForm.value);
     user.accountType = AccountType.Organisation;
     user.organisationId = this.currentOrganisationId;

       if (this.userForm.value['id'] !== null) {
        this.manageAdminOrganisationService.updateOrganisationUser(this.currentOrganisationId, user.id, user).subscribe({
          next: (updatedUser) => {
            console.log('User updated successfully' + updatedUser) //TODO show success toaster message 
            this.router.navigate(['/organisation-admin/manage-organisation-users']);
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       } else {      
        this.manageAdminOrganisationService.createOrganisationUserAccount(this.currentOrganisationId, user).subscribe({
          next: (newUser: IUser) => {
            if (newUser) {
              console.log(newUser) //TODO show success toaster
              this.router.navigate(['/organisation-admin/manage-organisation-users']);
            }
          },
          error: (error: ErrorResponse) => {
            console.log(error) // TODO: show error toaster
          }
        })
       }
     }
   }

 cancel() {
  this.router.navigate(['/organisation-admin/manage-organisation-users']);
 }

 private disableControl() {
   this.userForm.controls['password'].disable();
    this.userForm.controls['confirmPassword'].disable();
 }



}
