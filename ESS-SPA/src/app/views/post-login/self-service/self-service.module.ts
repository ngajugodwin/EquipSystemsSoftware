import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelfServiceComponent } from './self-service.component';
import { SelfServiceRoutingModule } from './self-service-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { CustomModule } from '../../../shared/modules/custom.module';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { CardModule, FormModule, GridModule } from '@coreui/angular';


@NgModule({
  imports: [
    CommonModule,
    SelfServiceRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    FormModule,
    GridModule,
    CustomModule,
    CardModule,
    ReactiveFormsModule
  ],
  declarations: [
      SelfServiceComponent,
      ProfileComponent,
      ChangePasswordComponent
  ],
  entryComponents: [
  ],
  providers: [
  ]
})
export class SelfServiceModule { }