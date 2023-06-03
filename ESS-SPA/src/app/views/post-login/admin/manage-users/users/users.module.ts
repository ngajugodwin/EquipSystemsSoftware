import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import {UsersComponent} from '../users/users.component';
import {UserComponent} from '../users/user/user.component';

import {UsersRoutingModule } from './users-routing.module'
import {
  ButtonModule,
    CardModule,
    FormModule,
    GridModule,
    TableModule,
  } from '@coreui/angular';
  
import { IconModule } from '@coreui/icons-angular';

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
ButtonModule
],

exports: [

], 
declarations:[
    UsersComponent,
    UserComponent
],
providers:[

]
})

export class UsersModule{}