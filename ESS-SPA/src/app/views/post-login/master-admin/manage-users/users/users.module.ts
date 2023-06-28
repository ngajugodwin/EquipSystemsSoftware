import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {UsersComponent} from '../users/users.component';
import {UserComponent} from '../users/user/user.component';
import {UserReviewComponent} from './user-review/user-review.component';

import {UsersRoutingModule } from './users-routing.module'
import {
  AvatarModule,
  BadgeModule,
  ButtonModule,
    CardModule,
    DropdownModule,
    FormModule,
    GridModule,
    TableModule,
  } from '@coreui/angular';
  
import { IconModule } from '@coreui/icons-angular';
import { CustomModule } from 'src/app/shared/modules/custom.module';

@NgModule({
imports: [    
ReactiveFormsModule,
UsersRoutingModule,
CardModule,
FormModule,
IconModule,
CommonModule,
TableModule,
GridModule,
ButtonModule,
DropdownModule,
AvatarModule,
FormsModule,
CustomModule,
BadgeModule
],

exports: [

], 
declarations:[
    UsersComponent,
    UserComponent,
    UserReviewComponent
],
providers:[

]
})

export class UsersModule{}