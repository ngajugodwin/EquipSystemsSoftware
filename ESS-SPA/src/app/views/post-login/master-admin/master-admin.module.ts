import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { MasterAdminRoutingModule } from './master-admin-routing.module';
import { ManageOrganisationsComponent } from './manage-organisations/manage-organisations.component';
import { AvatarModule, BadgeModule, ButtonModule, CardModule, DropdownModule, FormModule, GridModule, ModalModule, TableModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { IconModule } from '@coreui/icons-angular';
import { UsersRoutingModule } from './manage-users/users/users-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomModule } from 'src/app/shared/modules/custom.module';


@NgModule({

    imports:[
        ReactiveFormsModule,
        UsersRoutingModule,
        CardModule,
        FormModule,
        IconModule,
        CommonModule,
        TableModule,
        GridModule,
        ButtonModule,
        MasterAdminRoutingModule,
        AvatarModule,
        DropdownModule,
        CustomModule,
        ModalModule,
        BadgeModule,
        FormsModule
    ], 
    exports:[

    ],
    declarations:[
        ManageOrganisationsComponent
    ],
    providers:[

    ]
})

export class MasterAdminModule {}