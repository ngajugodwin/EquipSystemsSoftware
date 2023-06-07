import { NgModule } from '@angular/core';
import { MasterAdminRoutingModule } from './master-admin-routing.module';
import { ManageOrganisationsComponent } from './manage-organisations/manage-organisations.component';
import { ButtonModule, CardModule, FormModule, GridModule, TableModule } from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { IconModule } from '@coreui/icons-angular';
import { UsersRoutingModule } from './manage-users/users/users-routing.module';
import { ReactiveFormsModule } from '@angular/forms';


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