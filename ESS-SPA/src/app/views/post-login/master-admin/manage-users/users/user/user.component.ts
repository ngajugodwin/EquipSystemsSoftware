import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { AbstractControlOptions, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AccountType } from 'src/app/entities/models/accountType';
import { IOrganisation, Organisation } from 'src/app/entities/models/organisation';
import { IUser } from 'src/app/entities/models/user';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  accountTypes: { id: number; name: string }[] = [];
  organisationProperties: { id: number; name: string }[] = [
    {id: 1, name: 'name'},
    {id: 2, name: 'address'},
    {id: 3, name: 'registration'},
    {id: 4, name: 'dateOfIncorporation'}
  ];
  isOrganisationForm = false;
  userForm: FormGroup;
  formTitle: string = 'New User';
  selectedAccountType: AccountType;
  
  
  constructor(private fb: FormBuilder,   private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.initAccountTypes(AccountType);
    this.initForm();
this.test(Organisation);
   // this.addOrganisationControls(0);
  }

  initForm() {
    this.userForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'), Validators.email]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      organisation: new FormArray([]),
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required],
    }, {validator: [this.passwordMatchValidator]} as AbstractControlOptions)
  }

  
  get organisationControl(): FormArray {
    return this.userForm.get("organisation") as FormArray;
  }

  addOrganisationControls(orgData: any) {
 
    for (let i = 0; i < orgData.length; i++) {
      console.log(orgData[i]);
       const control = new FormControl(orgData[i]);
       (<FormArray>this.userForm.get('organisation')).push(control);
      // console.log(this.userForm.controls);
    }

   // const serviceItenId = this.documentsControl.controls[index].get('service_item_id').value
    // this.organisationControl.insert(
    //   index + 1,
    //   this.fb.group({
    //     name: ['', Validators.required],
    //   })
    // );

   }

   getUser() {
    this.activatedRoute.data.subscribe(data => {
      const user = data['user'];
      if (user) {
        this.formTitle = 'Edit User';
        this.disableControl();
        this.assignValuesToControl(user);
      } else {
        this.formTitle = 'New User';
      }
     });
   }

   assignValuesToControl(user: IUser) {
    this.userForm.patchValue({
      id: user.id,
      username: user.userName,
      email: user.email || '',
      firstName: user.firstName || '',
      lastName: user.lastName || '',
    });
  }


  onAccountTypeSelected(data: any) {
   if (data.target.value == 2) {
    this.isOrganisationForm = true;
    this.addOrganisationControls(Organisation);
   } else {
    this.isOrganisationForm = false;

   }
  }

  save() {
    const user: IUser = Object.assign({}, this.userForm.value);
    user.organisation = Object.assign({}, this.userForm.controls['organisation'].value);
//set account type

    // switch(this.selectedAccountType){
    //   case AccountType.Individual: {
    //     console.log(this.selectedAccountType);
    //     break;
    //   }
    //   case AccountType.Organisation: {
    //     console.log(this.selectedAccountType);
    //     break;
        
    //   }
    //   case AccountType.Master: {
    //     console.log(this.selectedAccountType);
    //     break;
        
    //   } default: {
    //     console.log(this.selectedAccountType);
    //     break;
    //   }
    // }
    console.log(user);
  }

  initAccountTypes(status: any) {
    for (var n in status) {
      if (typeof status[n] === "number") {
        this.accountTypes.push({ id: <any>status[n], name: n });
      }
    }
  }

  test(type: any) {
    for (var n in type) {
      if (typeof type[n] === "number") {
        this.organisationProperties.push({ id: <any>type[n], name: n });
      }
    }
    console.log(this.organisationProperties);
  }

  
  private disableControl() {
    this.userForm.controls['password'].disable();
    this.userForm.controls['confirmPassword'].disable();
  }

  private clear() {
    this.userForm.reset();
  }

  
  private passwordMatchValidator(f: FormGroup) {
    return f.get('password')?.value === f.get('confirmPassword')?.value ? null : {'mismatch': true};
  }


}
