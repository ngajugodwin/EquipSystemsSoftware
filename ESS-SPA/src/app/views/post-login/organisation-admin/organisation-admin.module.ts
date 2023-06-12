import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AvatarModule, BadgeModule, ButtonModule, CardModule, DropdownModule, FormModule, GridModule, ModalModule, TableModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { IconModule } from '@coreui/icons-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomModule } from '../../../shared/modules/custom.module';
import {ManageOrganisationUsersComponent} from './manage-organisation-users/manage-organisation-users.component';
import {OrganisationUserComponent} from './manage-organisation-users/organisation-user/organisation-user.component';
import {OrganisationAdminRoutingModule} from './organisation-admin-routing.module';

@NgModule({

    imports:[
        ReactiveFormsModule,
        CardModule,
        FormModule,
        IconModule,
        CommonModule,
        TableModule,
        GridModule,
        ButtonModule,
        AvatarModule,
        DropdownModule,
        CustomModule,
        ModalModule,
        BadgeModule,
        FormsModule,

        OrganisationAdminRoutingModule
    ], 
    exports:[

    ],
    declarations:[
        ManageOrganisationUsersComponent,
        OrganisationUserComponent
    ],
    providers:[

    ]
})

export class OrganisationAdminModule {}